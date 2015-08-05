// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;
using System.Drawing.Graphics;
using System.IO;
using System.Diagnostics;

public partial class GraphicsUnitTests
{
    //perftests
    static Image pngdog = Png.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\pngdog.png");
    static Image pngcat = Png.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\pngcat.png");
    static Image transpocat = Png.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\transpocat.png");
    static Image jpgdog = Jpg.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\jpgdog.jpg");
    static Image jpgcat = Jpg.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\jpgcat.jpg");



    //[Fact]
    //public static void LoadJpg1()
    //{
    //    Console.WriteLine("LoadJpg1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    Image jpg = Jpg.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\jpgcat.jpg");
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void LoadPng1()
    //{
    //    Console.WriteLine("LoadPng1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    Image png = Png.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\pngcat.png");
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void SaveJpg1()
    //{
    //    Console.WriteLine("SaveJpg1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    Jpg.WriteToFile(jpgdog, @"C:\Users\t-xix\Pictures\PerfTestResults\jpgdog.jpg");
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void SavePng1()
    //{
    //    Console.WriteLine("SavePng1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    Png.WriteToFile(pngdog, @"C:\Users\t-xix\Pictures\PerfTestResults\pngdog.png");
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void ResizeJpg1()
    //{
    //    Console.WriteLine("ResizeJpg1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    jpgcat.Resize(100, 100);
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void ResizePng1()
    //{
    //    Console.WriteLine("ResizePng1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    pngcat.Resize(100, 100);
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void ChangeAlphaJpg1()
    //{
    //    Console.WriteLine("ChangeAlphaJpg1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    jpgcat.SetAlphaPercentage(0.5f);
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void ChangeAlphaPng1()
    //{

    //    Console.WriteLine("ChangeAlphaPng1");

    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    pngcat.SetAlphaPercentage(0.5);
    //    Png.WriteToFile(pngcat, @"C:\Users\t-xix\Pictures\PerfTestResults\MOVINGTOMANAGEDTEST.png");
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void DrawJpgOverJpg1()
    //{
    //    Console.WriteLine("DrawJpgOverJpg1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    jpgdog.Draw(jpgcat, 10, 10);
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    [Fact(Skip = "UNSKIP to start Perf Tests")]
    public static void DrawPngOverPng1()
    {
        Console.WriteLine("DrawPngOverPng1");
        Stopwatch sw = new Stopwatch();
        sw.Start();
        pngdog.Draw(transpocat, 200, 200);
        Png.WriteToFile(pngdog, @"C:\Users\t-xix\Pictures\PerfTestResults\drawtest.png");
        sw.Stop();
        TimeSpan elapsedTime = sw.Elapsed;
        Console.WriteLine(elapsedTime);
        Console.WriteLine("");
    }

    //[Fact]
    //public static void DrawJpgOverPng1()
    //{
    //    Console.WriteLine("DrawJpgOverPng1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    pngdog.Draw(jpgcat, 10, 10);
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void DrawPngOverJpg1()
    //{
    //    Console.WriteLine("DrawPngOverJpg1");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    jpgdog.Draw(pngcat, 10, 10);
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}

    //[Fact]
    //public static void LoadJpg100()
    //{
    //    Console.WriteLine("LoadJpg100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        Image jpg = Jpg.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\jpgcat.jpg");
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //    Console.WriteLine("");
    //}
    //[Fact]
    //public static void LoadPng100()
    //{
    //    Console.WriteLine("LoadPng100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        Image png = Png.Load(@"C:\Users\t-xix\Pictures\PerfTestImages\pngcat.png");
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}
    //[Fact]
    //public static void SaveJpg100()
    //{
    //    Console.WriteLine("SaveJpg100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        Jpg.WriteToFile(jpgdog, @"C:\Users\t-xix\Pictures\PerfTestResults\jpgdog.jpg");
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}
    //[Fact]
    //public static void LoadAndSavePng100()
    //{
    //    Console.WriteLine("LoadAndSavePng100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        Png.WriteToFile(pngdog, @"C:\Users\t-xix\Pictures\PerfTestResults\pngdog.png");
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}
    //[Fact]
    //public static void ResizeJpg100()
    //{
    //    Console.WriteLine("ResizeJpg100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        jpgcat.Resize(100, 100);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}

    //[Fact]
    //public static void ResizePng100()
    //{
    //    Console.WriteLine("ResizePng100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        pngcat.Resize(100, 100);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}

    //[Fact]
    //public static void ChangeAlphaJpg100()
    //{
    //    Console.WriteLine("ChangeAlphaJpg100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        jpgcat.SetAlphaPercentage(0.5f);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}

    //[Fact]
    //public static void ChangeAlphaPng100()
    //{
    //    Console.WriteLine("ChangeAlphaPng100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        pngcat.SetAlphaPercentage(0.5f);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}
    //[Fact]
    //public static void DrawJpgOverJpg100()
    //{
    //    Console.WriteLine("DrawJpgOverJpg100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        jpgdog.Draw(jpgcat, 10, 10);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}
    //[Fact]
    //public static void DrawPngOverPng100()
    //{
    //    Console.WriteLine("DrawPngOverPng100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        pngdog.Draw(pngcat, 10, 10);
    //    }
    //        sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}

    //[Fact]
    //public static void DrawJpgOverPng100()
    //{
    //    Console.WriteLine("DrawJpgOverPng100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        pngdog.Draw(jpgcat, 10, 10);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}

    //[Fact]
    //public static void DrawPngOverJpg100()
    //{
    //    Console.WriteLine("DrawPngOverJpg100");
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        jpgdog.Draw(pngcat, 10, 10);
    //    }
    //    sw.Stop();
    //    TimeSpan elapsedTime = sw.Elapsed;
    //    Console.WriteLine(elapsedTime);
    //}

    //private static void ValidateImage(Image img, int widthToCompare, int heightToCompare)
    //{
    //    Assert.Equal(widthToCompare, img.WidthInPixels);
    //    Assert.Equal(heightToCompare, img.HeightInPixels);
    //}

    ////[Fact]
    ////public void WhenCreatingAJpegFromAValidFileGiveAValidImage()

    /////* Tests Load(filepath) method */
    ////[Fact]
    ////public void WhenCreatingAJpegFromAMalformedPathThenThrowException()

    //[Fact]
    //public void WhenCreatingAPngFromAValidFileGiveAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
    //    Image fromFile = Png.Load(filepath);
    //    ValidateImage(fromFile, 220, 220);
    //}

    //[Fact]
    //public void WhenCreatingAPngFromAMalformedPathThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:Users\t-dahid\Pictures\TestPics\BlackCat.png";
    //    Assert.Throws<FileNotFoundException>(() => Png.Load(invalidFilepath));
    //}

    //[Fact]
    //public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareDog.jpg";
    //    Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    //}

    ///* Tests Load(stream) mehtod*/
    //[Fact]
    //public void WhenCreatingAJpegFromAValidStreamThenWriteAValidImageToFile()
    //{
    //    using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg", FileMode.Open))
    //    {
    //        Image fromStream = Jpg.Load(filestream);
    //        ValidateImage(fromStream, 400, 249);
    //        Jpg.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamWrite.jpg");
    //    }

    //}
    //[Fact]
    //public void WhenCreatingAPngFromAValidStreamThenWriteAValidImageToFile()
    //{
    //    using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png", FileMode.Open))
    //    {
    //        Image fromStream = Png.Load(filestream);
    //        ValidateImage(fromStream, 360, 362);
    //        Png.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamWrite.png");
    //    }
    //}

    ///* Test Resize */
    //[Fact]
    //public void WhenResizingEmptyImageDownThenGiveAValidatedResizedImage()
    //{
    //    Image emptyResizeSquare = Image.Create(100, 100);
    //    emptyResizeSquare = emptyResizeSquare.Resize(10, 10);
    //    ValidateImage(emptyResizeSquare, 10, 10);
    //}
    //[Fact]
    //public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    //{
    //    Image emptyResizeSquare = Image.Create(100, 100);
    //    emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
    //    ValidateImage(emptyResizeSquare, 200, 200);
    //}
    //[Fact]
    //public void WhenResizingJpegLoadedFromFileThenGiveAValidatedResizedImage()
    //{

    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
    //    Image fromFileResizeSquare = Png.Load(filepath);
    //    fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
    //    ValidateImage(fromFileResizeSquare, 200, 200);
    //    Png.WriteToFile(fromFileResizeSquare, @"C:\Users\t-dahid\Pictures\TestFromFileResizedWrite.jpg");
    //}
    //[Fact]
    //public void WhenResizingPngLoadedFromFileThenGiveAValidatedResizedImage()
    //{

    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
    //    Image fromFileResizeSquare = Png.Load(filepath);
    //    fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
    //    ValidateImage(fromFileResizeSquare, 200, 200);
    //    Png.WriteToFile(fromFileResizeSquare, @"C:\Users\t-dahid\Pictures\TestFromFileResizedWrite.png");
    //}
    //[Fact]
    //public void WhenResizingJpegLoadedFromStreamThenGiveAValidatedResizedImage()
    //{
    //    using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg", FileMode.Open))
    //    {
    //        Image fromStream = Jpg.Load(filestream);
    //        ValidateImage(fromStream, 400, 249);
    //        fromStream = fromStream.Resize(400, 400);
    //        ValidateImage(fromStream, 400, 400);
    //        Jpg.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamResizedWrite.jpg");
    //    }
    //}

    //[Fact]
    //public void WhenResizingPngLoadedFromStreamThenGiveAValidatedResizedImage()
    //{
    //    using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png", FileMode.Open))
    //    {
    //        Image fromStream = Png.Load(filestream);
    //        ValidateImage(fromStream, 360, 362);
    //        fromStream = fromStream.Resize(400, 400);
    //        ValidateImage(fromStream, 400, 400);
    //        Png.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamResizedWrite.png");
    //    }
    //}


    ///* Tests Writing to a file*/
    //[Fact]
    //public void WhenWritingABlankCreatedJpegToAValidFileWriteToAValidFile()
    //{
    //    Image emptyImage = Image.Create(10, 10);
    //    ValidateImage(emptyImage, 10, 10);
    //    Jpg.WriteToFile(emptyImage, @"C:\Users\t-dahid\Pictures\TESTBlankWrite.jpg");
    //}
    //[Fact]
    //public void WhenWritingABlankCreatedPngToAValidFileWriteToAValidFile()
    //{
    //    Image emptyImage = Image.Create(10, 10);
    //    ValidateImage(emptyImage, 10, 10);
    //    Png.WriteToFile(emptyImage, @"C:\Users\t-dahid\Pictures\TESTBlankWrite.png");
    //}
    //[Fact]
    //public void WhenWritingAJpegCreatedFromFileToAValidFileWriteAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
    //    Image fromFile = Png.Load(filepath);
    //    ValidateImage(fromFile, 600, 701);
    //    Png.WriteToFile(fromFile, @"C:\Users\t-dahid\Pictures\TestFileWrite.jpg");
    //}
    //[Fact]
    //public void WhenWritingAPngCreatedFromFileToAValidFileWriteAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
    //    Image fromFile = Png.Load(filepath);
    //    ValidateImage(fromFile, 220, 220);
    //    Png.WriteToFile(fromFile, @"C:\Users\t-dahid\Pictures\TestFileWrite.png");
    //}

    //[Fact]
    //public void WhenWritingAPngMadeTransparentToAValidFileWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(img, 220, 220);
    //    img.SetAlphaPercentage(.2);
    //    Png.WriteToFile(img, @"C:\Users\t-dahid\Pictures\TestFromFileFileTransparentWrite.png");

    //}

    //[Fact]
    //public void WhenWritingATransparentResizedPngToAValidFileWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(img, 220, 220);
    //    img.SetAlphaPercentage(.2);
    //    img = img.Resize(400, 400);
    //    ValidateImage(img, 400, 400);
    //    Png.WriteToFile(img, @"C:\Users\t-dahid\Pictures\TestFromFileFileTransparentResizeWrite.png");
    //}

    ///* Tests Writing to a Stream*/
    //[Fact]
    //public void WhenWritingABlankCreatedJpegToAValidStreamWriteToAValidStream()
    //{
    //    Image img = Image.Create(100, 100);
    //    using(MemoryStream stream = new MemoryStream())
    //    {
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Jpg.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestBlankStreamWrite.jpg");
    //    }
    //}
    //public void WhenWritingABlankCreatedPngToAValidStreamWriteToAValidStream()
    //{
    //    Image img = Image.Create(100, 100);
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestBlankStreamWrite.png");
    //    }
    //}
    //[Fact]
    //public void WhenWritingAJpegFromFileToAValidStreamWriteAValidImage()
    //{
    //    Image img = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Jpg.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamWrite.jpg");
    //    }
    //}
    //[Fact]
    //public void WhenWritingAPngCreatedFromFileToAValidStreamWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        Png.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamWrite.png");
    //    }
    //}

    //[Fact]
    //public void WhenWritingAResizedJpegToAValidStreamWriteAValidImage()
    //{
    //    Image img = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        img = img.Resize(40, 40);
    //        ValidateImage(img, 40, 40);
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Jpg.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamResizeWrite.jpg");
    //    }
    //}
    //[Fact]
    //public void WhenWritingAResizedPngToAValidStreamWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        //comment
    //        img = img.Resize(40, 40);
    //        Png.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamResizeWrite.png");
    //    }
    //}

    //[Fact]
    //public void WhenWritingAPngMadeTransparentToAValidStreamWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        ValidateImage(img, 360, 362);
    //        img.SetAlphaPercentage(.2);
    //        Png.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        ValidateImage(img2, 360, 362);
    //        Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamTransparentWrite.png");
    //    }
    //}

    //[Fact]
    //public void WhenWritingATransparentResizedPngToAValidStreamWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        ValidateImage(img, 360, 362);
    //        img.SetAlphaPercentage(.2);
    //        img = img.Resize(400, 400);
    //        ValidateImage(img, 400, 400);
    //        Png.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        ValidateImage(img2, 400, 400);
    //        Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamTransparentResizeWrite.png");
    //    }
    //}

    ///* Test Draw */
    //[Fact]
    //public void WhenDrawingTwoImagesWriteACorrectResult()
    //{
    //    //open yellow cat image
    //    Image yellowCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg");
    //    ValidateImage(yellowCat, 600, 701);
    //    //open black cat image
    //    Image blackCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat, 220, 220);
    //    //draw & Write
    //    yellowCat.Draw(blackCat, 0, 0);
    //    Jpg.WriteToFile(yellowCat, @"C:\Users\t-dahid\Pictures\DrawTest.png");
    //}
    ///* Test SetTransparency */
    //[Fact]
    //public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    //{
    //    //open black cat image
    //    Image blackCat0 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat0, 220, 220);
    //    blackCat0.SetAlphaPercentage(0);
    //    ValidateImage(blackCat0, 220, 220);
    //    Png.WriteToFile(blackCat0, @"C:\Users\t-dahid\Pictures\SetTransparencyTest0.png");

    //    Image blackCat1 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat1, 220, 220);
    //    blackCat1.SetAlphaPercentage(.7);
    //    ValidateImage(blackCat1, 220, 220);
    //    Png.WriteToFile(blackCat1, @"C:\Users\t-dahid\\Pictures\SetTransparencyTest1.png");

    //    Image blackCat2 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat2, 220, 220);
    //    blackCat2.SetAlphaPercentage(1);
    //    ValidateImage(blackCat2, 220, 220);
    //    Png.WriteToFile(blackCat2, @"C:\Users\t-dahid\\Pictures\SetTransparencyTest2.png");
    //}
    ///* Test Draw and Set Transparency */
    //[Fact]
    //public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    //{
    //    //black cat load
    //    Image blackCat = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat, 220, 220);
    //    blackCat.SetAlphaPercentage(0.5);
    //    //yellow cat load
    //    Image yellowCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg");
    //    ValidateImage(yellowCat, 600, 701);
    //    yellowCat.Draw(blackCat, 0, 0);
    //    ValidateImage(yellowCat, 600, 701);
    //    //write
    //    Png.WriteToFile(yellowCat, @"C:\Users\t-dahid\Pictures\DrawAndTransparencyTest.png");
    //}

    //private static void ValidateImage(Image img, int widthToCompare, int heightToCompare)
    //{
    //    Assert.Equal(widthToCompare, img.WidthInPixels);
    //    Assert.Equal(heightToCompare, img.HeightInPixels);
    //}

    ///* Tests Create Method */
    //[Fact]
    //public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    //{
    //    //create an empty 10x10 image
    //    Image emptyTenSquare = Image.Create(10, 10);
    //    ValidateImage(emptyTenSquare, 10, 10);
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingABlankImageWithNegativeHeightThenThrowException()
    //{
    //    Assert.Throws<InvalidOperationException>(() => Image.Create(1, -1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingABlankImageWithNegativeWidthThenThrowException()
    //{
    //    Assert.Throws<InvalidOperationException>(() => Image.Create(-1, 1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingABlankImageWithNegativeSizesThenThrowException()
    //{
    //    Assert.Throws<InvalidOperationException>(() => Image.Create(-1, -1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingABlankImageWithZeroHeightThenThrowException()
    //{
    //    Assert.Throws<InvalidOperationException>(() => Image.Create(1, 0));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingABlankImageWithZeroWidthThenThrowException()
    //{
    //    Assert.Throws<InvalidOperationException>(() => Image.Create(0, 1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingABlankImageWithZeroParametersThenThrowException()
    //{
    //    Assert.Throws<InvalidOperationException>(() => Image.Create(0, 0));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingAnImageFromAValidFileGiveAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
    //    Image fromFile = Jpg.Load(filepath);
    //    ValidateImage(fromFile, 600, 701);
    //}


    ///* Tests Load(filepath) method */
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingAnImageFromAMalformedPathThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
    //    Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareDog.jpg";
    //    Exception exception = Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:\Users\t-dahid\Documents\GitHub\corefxlab\src\System.Drawing.Graphics\text.txt";
    //    Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    //}

    ///* Tests Load(stream) mehtod*/
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingAnImageFromAValidStreamThenWriteAValidImageToFile()
    //{
    //    using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg", FileMode.Open))
    //    {
    //        Image fromStream = Jpg.Load(filestream);
    //        ValidateImage(fromStream, 400, 249);
    //        Jpg.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestWriteFromStream.jpg");
    //    }

    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    //{
    //    Stream stream = null;
    //    Assert.Throws<InvalidOperationException>(() => Png.Load(stream));
    //}

    ///* Test Resize */
    //[Fact]
    //public void WhenResizingEmptyImageDownThenGiveAValidatedResizedImage()
    //{
    //    Image emptyResizeSquare = Image.Create(100, 100);
    //    emptyResizeSquare = emptyResizeSquare.Resize(10, 10);
    //    ValidateImage(emptyResizeSquare, 10, 10);
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    //{
    //    Image emptyResizeSquare = Image.Create(100, 100);
    //    emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
    //    ValidateImage(emptyResizeSquare, 200, 200);
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageLoadedFromFileThenGiveAValidatedResizedImage()
    //{
    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
    //    Image fromFileResizeSquare = Png.Load(filepath);
    //    fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
    //    ValidateImage(fromFileResizeSquare, 200, 200);
    //    Png.WriteToFile(fromFileResizeSquare, @"C:\Users\t-dahid\Pictures\TestCatResized.png");
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageLoadedFromStreamThenGiveAValidatedResizedImage()
    //{
    //    using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg", FileMode.Open))
    //    {
    //        Image fromStream = Jpg.Load(filestream);
    //        ValidateImage(fromStream, 400, 249);
    //        fromStream = fromStream.Resize(400, 400);
    //        ValidateImage(fromStream, 400, 400);
    //        Jpg.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestWriteFromStreamResized.jpg");
    //    }
    //}

    ///* Testing Resize parameters */
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageGivenNegativeHeightThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    Assert.Throws<InvalidOperationException>(() => img.Resize(-1, 1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageGivenNegativeWidthThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    Assert.Throws<InvalidOperationException>(() => img.Resize(1, -1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageGivenNegativeSizesThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    Assert.Throws<InvalidOperationException>(() => img.Resize(-1, -1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageGivenZeroHeightThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    Assert.Throws<InvalidOperationException>(() => img.Resize(0, 1));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageGivenZeroWidthThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    Assert.Throws<InvalidOperationException>(() => img.Resize(1, 0));
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenResizingImageGivenZeroSizesThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    Assert.Throws<InvalidOperationException>(() => img.Resize(0, 0));
    //}

    ///* Tests Writing to a file*/
    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingABlankCreatedImageToAValidFileWriteToAValidFile()
    //{
    //    Image emptyImage = Image.Create(10, 10);
    //    ValidateImage(emptyImage, 10, 10);
    //    Jpg.WriteToFile(emptyImage, @"C:\Users\t-dahid\Pictures\TESTBlankWrite.jpg");
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingAnImageCreatedFromFileToAValidFileWriteAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
    //    Image fromFile = Jpg.Load(filepath);
    //    ValidateImage(fromFile, 600, 701);
    //    Jpg.WriteToFile(fromFile, @"C:\Users\t-dahid\Pictures\TestCatWrite.jpg");
    //}

    ///* Tests Writing to a Stream*/
    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingABlankCreatedImageToAValidStreamWriteToAValidStream()
    //{
    //    Image img = Image.Create(100, 100);
    //    using(MemoryStream stream = new MemoryStream())
    //    {
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Jpg.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestBlankStreamWrite.jpg");
    //    }
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingAnImageCreatedFromFileToAValidStreamWriteAValidImage()
    //{
    //    Image img = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Jpg.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamWrite.jpg");
    //    }
    //}

    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingAResizedImageToAValidStreamWriteAValidImage()
    //{
    //    Image img = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        img = img.Resize(40, 40);
    //        ValidateImage(img, 40, 40);
    //        Jpg.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Jpg.Load(stream);
    //        Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamResizeWrite.jpg");
    //    }
    //}
    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingAResizedPngToAValidStreamWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        img = img.Resize(40, 40);
    //        Png.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamResizeWrite.png");
    //    }
    //}

    //[Fact(Skip = "huhuhu")]
    //public void WhenWritingAnImageMadeTransparentToAValidStreamWriteAValidImage()
    //{
    //    Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        img.SetAlphaPercentage(.2);
    //        Png.WriteToStream(img, stream);
    //        stream.Position = 0;
    //        Image img2 = Png.Load(stream);
    //        Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamTransparentWrite.png");
    //    }

    //}

    ///* Test Draw */
    //[Fact(Skip = "huhuhu")]
    //public void WhenDrawingTwoImagesWriteACorrectResult()
    //{
    //    //open yellow cat image
    //    Image yellowCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg");
    //    ValidateImage(yellowCat, 600, 701);
    //    //open black cat image
    //    Image blackCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat, 220, 220);
    //    //draw & Write
    //    yellowCat.Draw(blackCat, 0, 0);
    //    Jpg.WriteToFile(yellowCat, @"C:\Users\t-dahid\Pictures\DrawTest.png");
    //}
    ///* Test SetTransparency */
    //[Fact(Skip = "huhuhu")]
    //public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    //{
    //    //open black cat image
    //    Image blackCat0 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat0, 220, 220);
    //    blackCat0.SetAlphaPercentage(0);
    //    ValidateImage(blackCat0, 220, 220);
    //    Png.WriteToFile(blackCat0, @"C:\Users\t-dahid\Pictures\SetTransparencyTest0.png");

    //    Image blackCat1 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat1, 220, 220);
    //    blackCat1.SetAlphaPercentage(.7);
    //    ValidateImage(blackCat1, 220, 220);
    //    Png.WriteToFile(blackCat1, @"C:\Users\t-dahid\\Pictures\SetTransparencyTest1.png");

    //    Image blackCat2 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat2, 220, 220);
    //    blackCat2.SetAlphaPercentage(1);
    //    ValidateImage(blackCat2, 220, 220);
    //    Png.WriteToFile(blackCat2, @"C:\Users\t-dahid\\Pictures\SetTransparencyTest2.png");
    //}
    ///* Test Draw and Set Transparency */
    //[Fact(Skip = "huhuhu")]
    //public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    //{
    //    //black cat load
    //    Image blackCat = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
    //    ValidateImage(blackCat, 220, 220);
    //    blackCat.SetAlphaPercentage(0.5);
    //    //yellow cat load
    //    Image yellowCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg");
    //    ValidateImage(yellowCat, 600, 701);
    //    yellowCat.Draw(blackCat, 0, 0);
    //    ValidateImage(yellowCat, 600, 701);
    //    //write
    //    Png.WriteToFile(yellowCat, @"C:\Users\t-dahid\Pictures\DrawAndTransparencyTest.png");
    //}
}




