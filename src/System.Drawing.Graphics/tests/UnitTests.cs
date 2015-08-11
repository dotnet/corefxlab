// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;
using System.Drawing.Graphics;
using System.IO;
using System.Diagnostics;
using System.Reflection;

public partial class GraphicsUnitTests
{
    /* Performance Tests */
    static StreamWriter streamwriter;
    static Stopwatch sw = new Stopwatch();
    static TimeSpan elapsedTime;
    static Image jpgcat;
    static Image pngcat;
    static Image jpgdog;
    static Image pngdog;


    [Fact]
    public static void RunAllTests()
    {
        int _trialsToRun = 1;
        for (int trialsRun = 0; trialsRun < _trialsToRun; trialsRun++)
        {
            string filename = "Trial" + (trialsRun + 1) + "Results.txt";
            FileStream fstream = new FileStream(Interop.PerformanceTestsResultsDirectory + filename, FileMode.Open);
            streamwriter = new StreamWriter(fstream);
            runTests(1);
            runTests(10);
            runTests(100);
            runTests(1000);
            runTests(5000);
            runTests(10000);
            streamwriter.Dispose();
            fstream.Dispose();
        }



    }

    public static void runTests(int numRuns)
    {

        WriteTestHeader(numRuns);
        //LoadFileJpg
        WriteCurrentTest("LoadFileJpeg", numRuns);
        LoadFileJpegPerfTest(numRuns);
        WriteStopWatch();
        //LoadFilePng
        WriteCurrentTest("LoadFilePng", numRuns);
        LoadFilePngPerfTest(numRuns);
        WriteStopWatch();
        //WriteFileJpg
        WriteCurrentTest("WriteFileJpeg", numRuns);
        WriteFileJpegPerfTest(numRuns);
        WriteStopWatch();
        //WriteFilePng
        WriteCurrentTest("LoadFilePng", numRuns);
        WriteFilePngPerfTest(numRuns);
        WriteStopWatch();
        //ResizeJpg            
        WriteCurrentTest("ResizeJpeg", numRuns);
        ResizeJpegPerfTest(numRuns);
        WriteStopWatch();
        //resize png
        WriteCurrentTest("ResizePng", numRuns);
        ResizePngPerfTest(numRuns);
        WriteStopWatch();
        //ChangeAlphaJpg
        WriteCurrentTest("ChangeAlphaJpeg", numRuns);
        ChangeAlphaJpegPerfTest(numRuns);
        WriteStopWatch();
        //ChangeAlphaPng
        WriteCurrentTest("ChangeAlphaPng", numRuns);
        ChangeAlphaPngPerfTest(numRuns);
        WriteStopWatch();
        //DrawJpgOverJpg       
        WriteCurrentTest("DrawJpegOverJpeg", numRuns);
        DrawJpegOverJpegPerfTest(numRuns);
        WriteStopWatch();
        //DrawPngOverPng
        WriteCurrentTest("DrawPngOverPng", numRuns);
        DrawPngOverPngPerfTest(numRuns);
        WriteStopWatch();
        //DrawJpgOverPng
        WriteCurrentTest("DrawJpegOverPng", numRuns);
        DrawJpegOverPngPerfTest(numRuns);
        WriteStopWatch();
        //DrawPngOverJpg
        WriteCurrentTest("DrawPngOverJpeg", numRuns);
        DrawPngOverJpegPerfTest(numRuns);
        WriteStopWatch();
        //LoadStreamJpg
        WriteCurrentTest("LoadStreamJpeg", numRuns);
        LoadStreamJpegPerfTest(numRuns);
        WriteStopWatch();
        //LoadStreamPng
        WriteCurrentTest("LoadStreamPng", numRuns);
        LoadStreamPngPerfTest(numRuns);
        WriteStopWatch();
        //WriteStreamJpg
        WriteCurrentTest("WriteStreamJpeg", numRuns);
        WriteStreamJpegPerfTest(numRuns);
        WriteStopWatch();
        //WriteStreamPng
        WriteCurrentTest("WriteStreamPng", numRuns);
        WriteStreamPngPerfTest(numRuns);
        WriteStopWatch();

    }
    private static void WriteTestHeader(int numRuns)
    {
        Console.WriteLine("");
        Console.WriteLine("~~~~~~~~~~~ {0} Runs ~~~~~~~~~~~", numRuns);
        Console.WriteLine("");
        streamwriter.WriteLine("");
        streamwriter.WriteLine("~~~~~~~~~~~ {0} Runs ~~~~~~~~~~~", numRuns);
        streamwriter.WriteLine("");
    }

    private static void WriteCurrentTest(string currentTest, int numRuns)
    {
        Console.WriteLine(currentTest + "{0}", numRuns);
        streamwriter.WriteLine("LoadFilePng{0}", numRuns);
    }

    private static void WriteStopWatch()
    {
        elapsedTime = sw.Elapsed;
        Console.WriteLine(elapsedTime);
        Console.WriteLine("");
        streamwriter.WriteLine(elapsedTime);
        streamwriter.WriteLine("");
        sw.Reset();
    }

    private static void LoadFileJpegPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            Image img = Jpg.Load(Interop.PerformanceTestJpegCat);
            sw.Stop();
            img.ReleaseStruct();
        }
    }

    private static void LoadFilePngPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
            {
                Console.WriteLine(i);
            }
            sw.Start();
            Image img = Png.Load(Interop.PerformanceTestPngCat);
            sw.Stop();
            img.ReleaseStruct();
        }
    }

    private static void WriteFileJpegPerfTest(int numRuns)
    {
        jpgdog = Jpg.Load(Interop.PerformanceTestJpegDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
            {
                Console.WriteLine(i);
            }
            sw.Start();
            Jpg.WriteToFile(jpgdog, Interop.PerformanceTestsResultsDirectory + "jpgdog.jpg");
            sw.Stop();
        }
        jpgdog.ReleaseStruct();
    }

    private static void WriteFilePngPerfTest(int numRuns)
    {
        pngdog = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);
            sw.Start();
            Png.WriteToFile(pngdog, Interop.PerformanceTestsResultsDirectory + "pngcat.png");
            sw.Stop();
        }
        pngdog.ReleaseStruct();
    }

    private static void ResizeJpegPerfTest(int numRuns)
    {
        jpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            Image img = jpgcat.Resize(100, 100);
            sw.Stop();
            img.ReleaseStruct();
        }
        jpgcat.ReleaseStruct();
    }

    private static void ResizePngPerfTest(int numRuns)
    {
        pngcat = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            Image img = pngcat.Resize(100, 100);
            sw.Stop();
            img.ReleaseStruct();
        }
        pngcat.ReleaseStruct();
    }

    private static void ChangeAlphaJpegPerfTest(int numRuns)
    {
        jpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            jpgcat.SetAlphaPercentage(0.5);
            sw.Stop();
        }
        jpgcat.ReleaseStruct();
    }

    private static void ChangeAlphaPngPerfTest(int numRuns)
    {
        pngcat = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            pngcat.SetAlphaPercentage(0.5);
            sw.Stop();
        }
        pngcat.ReleaseStruct();
    }

    private static void DrawJpegOverJpegPerfTest(int numRuns)
    {
        jpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        jpgdog = Jpg.Load(Interop.PerformanceTestJpegDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            jpgdog.Draw(jpgcat, 10, 10);
            sw.Stop();
        }
        jpgcat.ReleaseStruct();
        jpgdog.ReleaseStruct();
    }

    private static void DrawPngOverPngPerfTest(int numRuns)
    {
        pngcat = Png.Load(Interop.PerformanceTestPngCat);
        pngdog = Png.Load(Interop.PerformanceTestPngDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            pngdog.Draw(pngcat, 10, 10);
            sw.Stop();
        }
        pngcat.ReleaseStruct();
        pngdog.ReleaseStruct();
    }

    private static void DrawJpegOverPngPerfTest(int numRuns)
    {
        jpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        pngdog = Png.Load(Interop.PerformanceTestPngDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            pngdog.Draw(jpgcat, 10, 10);
            sw.Stop();
        }
        jpgcat.ReleaseStruct();
        pngdog.ReleaseStruct();
    }

    private static void DrawPngOverJpegPerfTest(int numRuns)
    {
        jpgdog = Jpg.Load(Interop.PerformanceTestJpegDog);
        pngcat = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            sw.Start();
            jpgdog.Draw(pngcat, 10, 10);
            sw.Stop();
        }
        jpgdog.ReleaseStruct();
        pngcat.ReleaseStruct();
    }

    private static void LoadStreamJpegPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            using (FileStream filestream = new FileStream(Interop.PerformanceTestJpegCat, FileMode.Open))
            {
                sw.Start();
                Image img = Jpg.Load(filestream);
                sw.Stop();
                img.ReleaseStruct();
                //filestream.Dispose();
            }

        }
    }

    private static void LoadStreamPngPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            using (FileStream filestream = new FileStream(Interop.PerformanceTestPngCat, FileMode.Open))
            {
                sw.Start();
                Image img = Png.Load(filestream);
                sw.Stop();
                img.ReleaseStruct();
                //filestream.Dispose();
            }
        }
    }

    private static void WriteStreamJpegPerfTest(int numRuns)
    {
        jpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            using (MemoryStream stream = new MemoryStream())
            {
                sw.Start();
                Jpg.WriteToStream(jpgcat, stream);
                sw.Stop();
            }
        }
        jpgcat.ReleaseStruct();
    }

    private static void WriteStreamPngPerfTest(int numRuns)
    {
        pngcat = Jpg.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine(i);

            using (MemoryStream stream = new MemoryStream())
            {
                sw.Start();
                Png.WriteToStream(pngcat, stream);
                sw.Stop();
            }
        }
        pngcat.ReleaseStruct();
    }





    /*----------------------Functionality Unit Tests------------------------------------*/


    private static void ValidateImage2(Image img, string embeddedFilepathName)
    {
        //
        //Assert.Equal(widthToCompare, img.WidthInPixels);
        //Assert.Equal(heightToCompare, img.HeightInPixels);
        Stream s = typeof(GraphicsUnitTests).GetTypeInfo().Assembly.GetManifestResourceStream(embeddedFilepathName);
        Stream a = new FileStream("path", FileMode.Open);
        Assert.Equal(s, a);
        //TODO: make this way better!!
    }

    private static void ValidateImage(Image img, int widthToCompare, int heightToCompare)
    {
        Assert.Equal(widthToCompare, img.WidthInPixels);
        Assert.Equal(heightToCompare, img.HeightInPixels);
        //TODO: make this way better!!
    }

    /* Tests Create Method */
    [Fact(Skip = "Running only Perf Tests")]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
        Image emptyTenSquare = Image.Create(10, 10);
        ValidateImage(emptyTenSquare, 10, 10);
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingABlankImageWithNegativeHeightThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(1, -1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingABlankImageWithNegativeWidthThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(-1, 1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingABlankImageWithNegativeSizesThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(-1, -1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingABlankImageWithZeroHeightThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(1, 0));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingABlankImageWithZeroWidthThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(0, 1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingABlankImageWithZeroParametersThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(0, 0));
    }


    /* Tests Load(filepath) method */
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAJpegFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"/home/ddcloud/Documents/SquareCat.jpg";
        Image fromFile = Jpg.Load(filepath);
        ValidateImage(fromFile, 600, 701);
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAPngFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"/home/ddcloud/Documents/BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAJpegFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/\SquareCat.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAPngFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/\BlackCat.png";
        Assert.Throws<FileNotFoundException>(() => Png.Load(invalidFilepath));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/SquareDog.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/text.txt";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }


    /* Tests Load(stream) mehtod*/
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    {
        Stream stream = null;
        Assert.Throws<InvalidOperationException>(() => Png.Load(stream));
    }


    /* Test Resize */
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingEmptyImageDownThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(10, 10);
        ValidateImage(emptyResizeSquare, 10, 10);
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
        ValidateImage(emptyResizeSquare, 200, 200);
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingJpegLoadedFromFileThenGiveAValidatedResizedImage()
    {

        string filepath = @"/home/ddcloud/Documents/SquareCat.jpg";
        Image fromFileResizeSquare = Jpg.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateImage(fromFileResizeSquare, 200, 200);
        //must be commented out later (1 job rule)
        Jpg.WriteToFile(fromFileResizeSquare, @"/home/ddcloud/Documents/TestFromFileResizedWrite.jpg");
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingPngLoadedFromFileThenGiveAValidatedResizedImage()
    {

        string filepath = @"/home/ddcloud/Documents/BlackCat.png";
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(400, 400);
        ValidateImage(fromFileResizeSquare, 400, 400);
        //must be commented out later (1 job rule)
        Png.WriteToFile(fromFileResizeSquare, @"/home/ddcloud/Documents/TestFromFileResizedWrite.png");
    }
    [Fact(Skip = "Running only Perf Tests")]
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

    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingImageGivenNegativeHeightThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(-1, 1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingImageGivenNegativeWidthThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(1, -1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingImageGivenNegativeSizesThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(-1, -1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingImageGivenZeroHeightThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(0, 1));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingImageGivenZeroWidthThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(1, 0));
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenResizingImageGivenZeroSizesThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(0, 0));
    }


    /* Test WriteToFile */
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenWritingABlankCreatedJpegToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Jpg.WriteToFile(emptyImage, @"/home/ddcloud/Documents/TestBlankWriteFile.jpg");
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenWritingABlankCreatedPngToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Png.WriteToFile(emptyImage, @"/home/ddcloud/Documents/TestBlankWriteFile.png");
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenWritingAJpegCreatedFromFileToAValidFileWriteAValidImage()
    {
        string filepath = @"/home/ddcloud/Documents/SquareCat.jpg";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 600, 701);
        Png.WriteToFile(fromFile, @"/home/ddcloud/Documents/TestFromFileWriteFile.jpg");
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenWritingAPngCreatedFromFileToAValidFileWriteAValidImage()
    {
        string filepath = @"/home/ddcloud/Documents/BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
        Png.WriteToFile(fromFile, @"/home/ddcloud/Documents/TestFromFileWriteFile.png");
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenWritingAPngMadeTransparentToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        Png.WriteToFile(img, @"/home/ddcloud/Documents/TestFromFileTransparentWriteFile.png");
    }
    [Fact(Skip = "Running only Perf Tests")]
    public void WhenWritingATransparentResizedPngToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        img = img.Resize(400, 400);
        ValidateImage(img, 400, 400);
        Png.WriteToFile(img, @"/home/ddcloud/Documents/TestFromFileTransparentResizeWriteFile.png");
    }
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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

    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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

    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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
    [Fact(Skip = "Running only Perf Tests")]
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






