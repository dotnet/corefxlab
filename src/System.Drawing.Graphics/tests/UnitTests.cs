// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;
using System.Drawing.Graphics;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

public partial class GraphicsUnitTests
{
    /* Performance Tests */
    static StreamWriter streamwriter;
    static Stopwatch stopwatchSingleThread = new Stopwatch();
    static Stopwatch stopwatchMultiThread = new Stopwatch();
    static double elapsedTime;
    //static Image jpgcat;
    //static Image pngcat;
    //static Image jpgdog;
    //static Image pngdog;

    [Fact]
    public static void RunAllTests()
    {
            string filename = "Trial1Results.txt";
            FileStream fstream = new FileStream(Interop.PerformanceTestsResultsDirectory + filename, FileMode.Open);
            streamwriter = new StreamWriter(fstream);
            runTests(1000);
            streamwriter.Dispose();
            fstream.Dispose();
    }

    public static void runTests(int numRuns)
    {
        WriteTestHeader(numRuns);
        //LoadFileJpg
        WriteCurrentTest("LoadFileJpeg", numRuns);
        LoadFileJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "LoadFileJpeg");
        //LoadFilePng
        WriteCurrentTest("LoadFilePng", numRuns);
        LoadFilePngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "LoadFilePng");
        //WriteFileJpg
        WriteCurrentTest("WriteFileJpeg", numRuns);
        WriteFileJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteFileJpeg");
        //WriteFilePng
        WriteCurrentTest("WriteFilePng", numRuns);
        WriteFilePngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteFilePng");
        //ResizeJpg            
        WriteCurrentTest("ResizeJpeg", numRuns);
        ResizeJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "ResizeJpeg");
        //resize png
        WriteCurrentTest("ResizePng", numRuns);
        ResizePngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "ResizePng");
        //ChangeAlphaJpg
        WriteCurrentTest("ChangeAlphaJpeg", numRuns);
        ChangeAlphaJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "ChangeAlphaJpeg");
        //ChangeAlphaPng
        WriteCurrentTest("ChangeAlphaPng", numRuns);
        ChangeAlphaPngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "ChangeAlphaPng");
        //DrawJpgOverJpg       
        WriteCurrentTest("DrawJpegOverJpeg", numRuns);
        DrawJpegOverJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "DrawJpegOverJpeg");
        //DrawPngOverPng
        WriteCurrentTest("DrawPngOverPng", numRuns);
        DrawPngOverPngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "DrawPngOverPng");
        //DrawJpgOverPng
        WriteCurrentTest("DrawJpegOverPng", numRuns);
        DrawJpegOverPngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "DrawJpegOverPng");
        //DrawPngOverJpg
        WriteCurrentTest("DrawPngOverJpeg", numRuns);
        DrawPngOverJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "DrawPngOverJpeg");
        //LoadStreamJpg
        WriteCurrentTest("LoadStreamJpeg", numRuns);
        LoadStreamJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "LoadStreamJpeg");
        //LoadStreamPng
        WriteCurrentTest("LoadStreamPng", numRuns);
        LoadStreamPngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "LoadStreamPng");
        //WriteStreamJpg
        WriteCurrentTest("WriteStreamJpeg", numRuns);
        WriteStreamJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteStreamJpeg");
        //WriteStreamPng
        WriteCurrentTest("WriteStreamPng", numRuns);
        WriteStreamPngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteStreamPng");


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
        streamwriter.WriteLine(currentTest + "{0}", numRuns);
    }

    private static void WriteStopWatch(Stopwatch sw, string currentTest)
    {
        double elapsedSecs = (sw.ElapsedMilliseconds)/(1000.0);
        Console.WriteLine(elapsedSecs);
        Console.WriteLine("");
        streamwriter.WriteLine("Elapsed time for " + currentTest + ": " + elapsedSecs);
        streamwriter.WriteLine("");
        sw.Reset();
    }
    [Fact]
    public static void RunAllPerfTestsWithThreads()
    {
        int numOfTasks = 8;
        int numRuns = 1000;
        FileStream fstream = new FileStream(Interop.PerformanceTestsResultsDirectory + "Trial2Results.txt", FileMode.Open);
        streamwriter = new StreamWriter(fstream);
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "LoadFileJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "LoadFilePngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WriteFileJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WriteFilePngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "ResizeJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "ResizePngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "ChangeAlphaJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "ChangeAlphaPngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "DrawJpegOverJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "DrawPngOverPngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "DrawJpegOverPngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "DrawPngOverJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "LoadStreamJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "LoadStreamPngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WriteStreamJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WriteStreamPngPerfTest");

        streamwriter.Dispose();
        fstream.Dispose();

    }

    private static void RunOneFuntionWithMultipleTasks(int numOfTasks, int numRuns, string functionToRun)
    {
        WriteCurrentTest(functionToRun, numRuns);
        Task[] tasks = new Task[numOfTasks];
        stopwatchMultiThread.Start();
        for (int i = 0; i < numOfTasks; i++)
        {
            switch (functionToRun)
            {
                case "LoadFileJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => LoadFileJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "LoadFilePngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => LoadFilePngPerfTest(numRuns / numOfTasks));
                    break;
                case "WriteFileJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => WriteFileJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "WriteFilePngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => WriteFilePngPerfTest(numRuns / numOfTasks));
                    break;
                case "ResizeJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => ResizeJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "ResizePngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => ResizePngPerfTest(numRuns / numOfTasks));
                    break;
                case "ChangeAlphaJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => ChangeAlphaJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "ChangeAlphaPngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => ChangeAlphaPngPerfTest(numRuns / numOfTasks));
                    break;
                case "DrawJpegOverJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => DrawJpegOverJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "DrawPngOverPngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => DrawPngOverPngPerfTest(numRuns / numOfTasks));
                    break;
                case "DrawJpegOverPngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => DrawJpegOverPngPerfTest(numRuns / numOfTasks));
                    break;
                case "DrawPngOverJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => DrawPngOverJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "LoadStreamJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => LoadStreamJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "LoadStreamPngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => LoadStreamPngPerfTest(numRuns / numOfTasks));
                    break;
                case "WriteStreamJpegPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => WriteStreamJpegPerfTest(numRuns / numOfTasks));
                    break;
                case "WriteStreamPngPerfTest":
                    tasks[i] = Task.Factory.StartNew(() => WriteStreamPngPerfTest(numRuns / numOfTasks));
                    break;
                default:
                    throw new NotSupportedException("A task was created but not given a proper task. Check the code/swithc statement.");
            }
            //Console.WriteLine("Starting Task {0}...", i);
        }
        Task.WaitAll(tasks);
        stopwatchMultiThread.Stop();
        WriteStopWatch(stopwatchMultiThread, functionToRun);
    }

    private static void LoadFileJpegPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("LoadFileJpegTest :" + i);

            stopwatchSingleThread.Start();
            Image img = Jpg.Load(Interop.PerformanceTestJpegCat);
            stopwatchSingleThread.Stop();
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
                Console.WriteLine("LoadFilePngTest :" + i);
            }
            stopwatchSingleThread.Start();
            Image img = Png.Load(Interop.PerformanceTestPngCat);
            stopwatchSingleThread.Stop();
            img.ReleaseStruct();
        }
    }
    //FIX Write
    private static void WriteFileJpegPerfTest(int numRuns)
    {
        Image _thisjpgdog = Jpg.Load(Interop.PerformanceTestJpegDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
            {
                Console.WriteLine("WriteFileJpegTest :" + i);
            }
            stopwatchSingleThread.Start();
            Jpg.WriteToFile(_thisjpgdog, Interop.PerformanceTestsResultsDirectory + "jpgdog.jpg");
            stopwatchSingleThread.Stop();
        }
        _thisjpgdog.ReleaseStruct();
    }

    //fix write
    private static void WriteFilePngPerfTest(int numRuns)
    {
        Image _thispngdog = Png.Load(Interop.PerformanceTestPngDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("WriteFilePngTest :" + i);
            stopwatchSingleThread.Start();
            Png.WriteToFile(_thispngdog, Interop.PerformanceTestsResultsDirectory + "pngdog.png");
            stopwatchSingleThread.Stop();
        }
        _thispngdog.ReleaseStruct();
    }

    private static void ResizeJpegPerfTest(int numRuns)
    {
        Image _thisjpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("ResizeJpegTest :" + i);

            stopwatchSingleThread.Start();
            Image img = _thisjpgcat.Resize(100, 100);
            stopwatchSingleThread.Stop();
            img.ReleaseStruct();
        }
        _thisjpgcat.ReleaseStruct();
    }

    private static void ResizePngPerfTest(int numRuns)
    {
        Image _thispngcat = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("ResizePngTest :" + i);

            stopwatchSingleThread.Start();
            Image img = _thispngcat.Resize(100, 100);
            stopwatchSingleThread.Stop();
            img.ReleaseStruct();
        }
        _thispngcat.ReleaseStruct();
    }

    private static void ChangeAlphaJpegPerfTest(int numRuns)
    {
        Image _thisjpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("ChangeAlphaJpegTest :" + i);

            stopwatchSingleThread.Start();
            _thisjpgcat.SetAlphaPercentage(0.5);
            stopwatchSingleThread.Stop();
        }
        _thisjpgcat.ReleaseStruct();
    }

    private static void ChangeAlphaPngPerfTest(int numRuns)
    {
        Image _thispngcat = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("ChangeAlphaPngTest :" + i);

            stopwatchSingleThread.Start();
            _thispngcat.SetAlphaPercentage(0.5);
            stopwatchSingleThread.Stop();
        }
        _thispngcat.ReleaseStruct();
    }

    private static void DrawJpegOverJpegPerfTest(int numRuns)
    {
        Image _thisjpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        Image _thisjpgdog = Jpg.Load(Interop.PerformanceTestJpegDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("DrawJpegOverJpegTest :" + i);

            stopwatchSingleThread.Start();
            _thisjpgdog.Draw(_thisjpgcat, 10, 10);
            stopwatchSingleThread.Stop();
        }
        _thisjpgcat.ReleaseStruct();
        _thisjpgdog.ReleaseStruct();
    }

    private static void DrawPngOverPngPerfTest(int numRuns)
    {
        Image _thispngcat = Png.Load(Interop.PerformanceTestPngCat);
        Image _thispngdog = Png.Load(Interop.PerformanceTestPngDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("DrawPngOverPngTest :" + i);

            stopwatchSingleThread.Start();
            _thispngdog.Draw(_thispngcat, 10, 10);
            stopwatchSingleThread.Stop();
        }
        _thispngcat.ReleaseStruct();
        _thispngdog.ReleaseStruct();
    }

    private static void DrawJpegOverPngPerfTest(int numRuns)
    {
        Image _thisjpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        Image _thispngdog = Png.Load(Interop.PerformanceTestPngDog);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("DrawJpegOverPngTest :" + i);

            stopwatchSingleThread.Start();
            _thispngdog.Draw(_thisjpgcat, 10, 10);
            stopwatchSingleThread.Stop();
        }
        _thisjpgcat.ReleaseStruct();
        _thispngdog.ReleaseStruct();
    }

    private static void DrawPngOverJpegPerfTest(int numRuns)
    {
        Image _thisjpgdog = Jpg.Load(Interop.PerformanceTestJpegDog);
        Image _thispngcat = Png.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("DrawPngOverJpegTest :" + i);

            stopwatchSingleThread.Start();
            _thisjpgdog.Draw(_thispngcat, 10, 10);
            stopwatchSingleThread.Stop();
        }
        _thisjpgdog.ReleaseStruct();
        _thispngcat.ReleaseStruct();
    }

    private static void LoadStreamJpegPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("LoadStreamJpegTest :" + i);

            using (FileStream filestream = new FileStream(Interop.PerformanceTestJpegCat, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stopwatchSingleThread.Start();
                Image img = Jpg.Load(filestream);
                stopwatchSingleThread.Stop();
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
                Console.WriteLine("LoadStreamPngTest :" + i);

            //fixed stream by giving acces to multiple threads?
            using (FileStream filestream = new FileStream(Interop.PerformanceTestPngCat, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stopwatchSingleThread.Start();
                Image img = Png.Load(filestream);
                stopwatchSingleThread.Stop();
                img.ReleaseStruct();
                //filestream.Dispose();
            }
        }
    }

    private static void WriteStreamJpegPerfTest(int numRuns)
    {
        Image _thisjpgcat = Jpg.Load(Interop.PerformanceTestJpegCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("WriteStreamJpegTest :" + i);

            using (MemoryStream stream = new MemoryStream())
            {
                stopwatchSingleThread.Start();
                Jpg.WriteToStream(_thisjpgcat, stream);
                stopwatchSingleThread.Stop();
            }
        }
        _thisjpgcat.ReleaseStruct();
    }

    private static void WriteStreamPngPerfTest(int numRuns)
    {
        Image _thispngcat = Jpg.Load(Interop.PerformanceTestPngCat);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("WriteStreamPngTest :" + i);

            using (MemoryStream stream = new MemoryStream())
            {
                stopwatchSingleThread.Start();
                Png.WriteToStream(_thispngcat, stream);
                stopwatchSingleThread.Stop();
            }
        }
        _thispngcat.ReleaseStruct();
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






