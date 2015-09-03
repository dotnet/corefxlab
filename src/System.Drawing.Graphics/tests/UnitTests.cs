// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//#define PERFORMANCE_TESTING
using System;
using Xunit;
using System.Drawing.Graphics;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

public partial class GraphicsUnitTests
{

    /* Functionality test Constants */
    static string SquareCatLogicalName = "SquareCatJpeg";
    static string BlackCatLogicalName = "BlackCatPng";
    static string SoccerCatLogicalName = "SoccerCatJpeg";
    static string CuteCatLogicalName = "CuteCatPng";
    static string JpegCatLogicalName = "JpegCat";
    static string PngCatLogicalName = "PngCat";

    /* Performance Tests Constants */
#if PERFORMANCE_TESTING
    static StreamWriter streamwriter;
    static Stopwatch stopwatchSingleThread = new Stopwatch();
    static Stopwatch stopwatchMultiThread = new Stopwatch();
    /* Performance Tests Variables */
    static string jpegCatPath = "";
    static string jpegDogPath = "";
    static string pngCatPath = "";
    static string pngDogPath = "";
#endif

    /*----------------------Functionality Unit Tests------------------------------------*/
    private static void ValidateImagePng(Image img, string embeddedLogicalName)
    {
        Stream toCompare = typeof(GraphicsUnitTests).GetTypeInfo().Assembly.GetManifestResourceStream(embeddedLogicalName);
        Image comparison = Png.Load(toCompare);
        Assert.Equal(comparison.HeightInPixels, img.HeightInPixels);
        Assert.Equal(comparison.WidthInPixels, img.WidthInPixels);
        Assert.Equal(comparison.TrueColor, img.TrueColor);
    }

    private static void ValidateImageJpeg(Image img, string embeddedLogicalName)
    {
        Stream toCompare = typeof(GraphicsUnitTests).GetTypeInfo().Assembly.GetManifestResourceStream(embeddedLogicalName);
        Image comparison = Jpg.Load(toCompare);
        Assert.Equal(comparison.HeightInPixels, img.HeightInPixels);
        Assert.Equal(comparison.WidthInPixels, img.WidthInPixels);
        Assert.Equal(comparison.TrueColor, img.TrueColor);
    }

    private static void ValidateCreatedImage(Image img, int widthToCompare, int heightToCompare)
    {
        Assert.Equal(widthToCompare, img.WidthInPixels);
        Assert.Equal(heightToCompare, img.HeightInPixels);
    }

    private static string ChooseExtension(string filepath)
    {
        if (filepath.Contains("Jpeg"))
            return ".jpg";
        else
            return ".png";
    }


    private static string SaveEmbeddedResourceToFile(string logicalName)
    {
        //get a temp file path
        string toReturn = Path.GetTempFileName();
        toReturn = Path.ChangeExtension(toReturn, ChooseExtension(logicalName));
        //get stream of embedded resoruce
        Stream embeddedResourceStream = typeof(GraphicsUnitTests).GetTypeInfo().Assembly.GetManifestResourceStream(logicalName);
        //write stream to temp file path
        using (FileStream fileStream = new FileStream(toReturn, FileMode.OpenOrCreate))
        {
            embeddedResourceStream.Seek(0, SeekOrigin.Begin);
            embeddedResourceStream.CopyTo(fileStream);
        }
        //return where the resource is saved
        return toReturn;
    }

    /* Tests Create Method */
    [Fact]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
        Image emptyTenSquare = Image.Create(10, 10);
        ValidateCreatedImage(emptyTenSquare, 10, 10);
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
        //save embedded resource to a file
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);
        //read it
        Image newJpeg = Jpg.Load(filepath);
        File.Delete(filepath);
        //validate it
        ValidateImageJpeg(newJpeg, SquareCatLogicalName);
    }
    [Fact]
    public void WhenCreatingAPngFromAValidFileGiveAValidImage()
    {
        //save embedded resource to a file
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        //read it
        Image newJpeg = Png.Load(filepath);
        File.Delete(filepath);
        //validate it
        ValidateImagePng(newJpeg, BlackCatLogicalName);
    }
    [Fact]
    public void WhenCreatingAJpegFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string temporaryPath = Path.GetTempPath();
        string invalidFilepath = temporaryPath + "\\Hi.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAPngFromAMalformedPathThenThrowException()
    {
        string temporaryPath = Path.GetTempPath();
        string invalidFilepath = temporaryPath + "\\Hi.png";
        Assert.Throws<FileNotFoundException>(() => Png.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        string temporaryPath = Path.GetTempPath();
        string invalidFilepath = temporaryPath + "\\Hi.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        string temporaryPath = Path.GetTempPath();
        string invalidFilepath = temporaryPath + "text.txt";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }


    /* Tests Load(stream) mehtod*/
    [Fact]
    public void WhenCreatingAJpegFromAValidStreamThenWriteAValidImageToFile()
    {
        string filepath = SaveEmbeddedResourceToFile(SoccerCatLogicalName);
        using (FileStream filestream = new FileStream(filepath, FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImageJpeg(fromStream, SoccerCatLogicalName);
        }
        File.Delete(filepath);
    }
    [Fact]
    public void WhenCreatingAPngFromAValidStreamThenWriteAValidImageToFile()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        using (FileStream filestream = new FileStream(filepath, FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImagePng(fromStream, CuteCatLogicalName);
        }
        File.Delete(filepath);
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
        ValidateCreatedImage(emptyResizeSquare, 10, 10);
    }
    [Fact]
    public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
        ValidateCreatedImage(emptyResizeSquare, 200, 200);
    }
    [Fact]
    public void WhenResizingJpegLoadedFromFileThenGiveAValidatedResizedImage()
    {
        //what to do? Have embedded resource stream of expected result?
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);
        Image fromFileResizeSquare = Jpg.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateCreatedImage(fromFileResizeSquare, 200, 200);
        File.Delete(filepath);
    }
    [Fact]
    public void WhenResizingPngLoadedFromFileThenGiveAValidatedResizedImage()
    {
        //what to do? Have embedded resource stream of expected result?
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(400, 400);
        ValidateCreatedImage(fromFileResizeSquare, 400, 400);
        File.Delete(filepath);
    }
    [Fact]
    public void WhenResizingJpegLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SoccerCatLogicalName);
        using (FileStream filestream = new FileStream(filepath, FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            fromStream = fromStream.Resize(400, 400);
            ValidateCreatedImage(fromStream, 400, 400);
        }
        File.Delete(filepath);
    }

    [Fact]
    public void WhenResizingPngLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        using (FileStream filestream = new FileStream(filepath, FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            fromStream = fromStream.Resize(400, 400);
            ValidateCreatedImage(fromStream, 400, 400);
        }
        File.Delete(filepath);
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


    /* Test Write */
    [Fact]
    public void WhenWritingABlankCreatedJpegToAValidFileWriteAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateCreatedImage(emptyImage, 10, 10);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");
        Jpg.Write(emptyImage, tempFilePath);
        File.Delete(tempFilePath);

    }
    [Fact]
    public void WhenWritingABlankCreatedPngToAValidFileWriteAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateCreatedImage(emptyImage, 10, 10);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".png");
        Png.Write(emptyImage, tempFilePath);
        File.Delete(tempFilePath);

    }
    [Fact]
    public void WhenWritingAJpegCreatedFromFileToAValidFileWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);
        Image fromFile = Jpg.Load(filepath);
        ValidateImageJpeg(fromFile, SquareCatLogicalName);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");
        Png.Write(fromFile, tempFilePath);
        File.Delete(filepath);
        File.Delete(tempFilePath);
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidFileWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image fromFile = Png.Load(filepath);
        ValidateImagePng(fromFile, BlackCatLogicalName);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".png");
        Png.Write(fromFile, tempFilePath);
        File.Delete(filepath);
        File.Delete(tempFilePath);
    }
    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidFileWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image img = Png.Load(filepath);
        ValidateImagePng(img, BlackCatLogicalName);
        img.SetAlphaPercentage(.2);
        ValidateImagePng(img, BlackCatLogicalName);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".png");
        Png.Write(img, tempFilePath);
        File.Delete(filepath);
        File.Delete(tempFilePath);
    }
    [Fact]
    public void WhenWritingATransparentResizedPngToAValidFileWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image img = Png.Load(filepath);
        ValidateImagePng(img, BlackCatLogicalName);
        img.SetAlphaPercentage(.2);
        img = img.Resize(400, 400);
        ValidateCreatedImage(img, 400, 400);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".png");
        Png.Write(img, tempFilePath);
        File.Delete(filepath);
        File.Delete(tempFilePath);
    }
    [Fact]
    public void WhenWritingAResizedTransparentPngToAValidFileWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image img = Png.Load(filepath);
        ValidateImagePng(img, BlackCatLogicalName);
        img = img.Resize(400, 400);
        ValidateCreatedImage(img, 400, 400);
        img.SetAlphaPercentage(.2);
        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".png");
        Png.Write(img, tempFilePath);
        File.Delete(filepath);
        File.Delete(tempFilePath);
    }

    /* Tests Writing to a Stream*/
    [Fact]
    public void WhenWritingABlankCreatedJpegToAValidStreamWriteAValidStream()
    {
        Image img = Image.Create(100, 100);
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.Write(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            ValidateCreatedImage(img2, 100, 100);
        }
    }
    [Fact]
    public void WhenWritingABlankCreatedPngToAValidStreamWriteAValidStream()
    {
        Image img = Image.Create(100, 100);
        using (MemoryStream stream = new MemoryStream())
        {
            Png.Write(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateCreatedImage(img2, 100, 100);
        }
    }
    [Fact]
    public void WhenWritingAJpegFromFileToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SoccerCatLogicalName);
        Image img = Jpg.Load(filepath);
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.Write(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            ValidateImageJpeg(img2, SoccerCatLogicalName);
        }
        File.Delete(filepath);
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        Image img = Png.Load(filepath);
        using (MemoryStream stream = new MemoryStream())
        {
            Png.Write(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImagePng(img2, CuteCatLogicalName);
        }
        File.Delete(filepath);
    }

    [Fact]
    public void WhenWritingAResizedJpegToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SoccerCatLogicalName);
        Image img = Jpg.Load(filepath);
        using (MemoryStream stream = new MemoryStream())
        {
            img = img.Resize(40, 40);
            ValidateCreatedImage(img, 40, 40);
            Jpg.Write(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            ValidateCreatedImage(img, 40, 40);
        }
        File.Delete(filepath);
    }
    [Fact]
    public void WhenWritingAResizedPngToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        Image img = Png.Load(filepath);
        using (MemoryStream stream = new MemoryStream())
        {
            img = img.Resize(40, 40);
            ValidateCreatedImage(img, 40, 40);
            Png.Write(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateCreatedImage(img, 40, 40);
        }
        File.Delete(filepath);
    }

    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        Image img = Png.Load(filepath);
        using (MemoryStream stream = new MemoryStream())
        {
            img.SetAlphaPercentage(.2);
            ValidateImagePng(img, CuteCatLogicalName);
            Png.Write(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImagePng(img2, CuteCatLogicalName);
        }
        File.Delete(filepath);
    }
    [Fact]
    public void WhenWritingATransparentResizedPngToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        Image img = Png.Load(filepath);
        using (MemoryStream stream = new MemoryStream())
        {
            img.SetAlphaPercentage(.2);
            img = img.Resize(400, 400);
            ValidateCreatedImage(img, 400, 400);
            Png.Write(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateCreatedImage(img2, 400, 400);
        }
        File.Delete(filepath);
    }
    [Fact]
    public void WhenWritingAResizedTransparentPngToAValidStreamWriteAValidImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        Image img = Png.Load(filepath);
        ValidateImagePng(img, CuteCatLogicalName);
        img = img.Resize(400, 400);
        ValidateCreatedImage(img, 400, 400);
        img.SetAlphaPercentage(.2);
        File.Delete(filepath);
    }

    /* Test Draw */
    [Fact]
    public void WhenDrawingTwoImagesWriteACorrectResult()
    {
        //open yellow cat image
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);
        Image yellowCat = Jpg.Load(filepath);
        ValidateImageJpeg(yellowCat, SquareCatLogicalName);
        //open black cat image
        string filepath2 = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image blackCat = Jpg.Load(filepath2);
        ValidateImagePng(blackCat, BlackCatLogicalName);
        //draw
        yellowCat.Draw(blackCat, 0, 0);
        ValidateImageJpeg(yellowCat, SquareCatLogicalName);
        File.Delete(filepath);
        File.Delete(filepath2);
    }
    /* Test SetTransparency */
    [Fact]
    public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    {
        //open black cat image
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image blackCat0 = Jpg.Load(filepath);
        ValidateImagePng(blackCat0, BlackCatLogicalName);
        blackCat0.SetAlphaPercentage(0);
        ValidateImagePng(blackCat0, BlackCatLogicalName);

        Image blackCat1 = Jpg.Load(filepath);
        ValidateImagePng(blackCat1, BlackCatLogicalName);
        blackCat0.SetAlphaPercentage(0.5);
        ValidateImagePng(blackCat1, BlackCatLogicalName);

        Image blackCat2 = Jpg.Load(filepath);
        ValidateImagePng(blackCat2, BlackCatLogicalName);
        blackCat0.SetAlphaPercentage(1);
        ValidateImagePng(blackCat2, BlackCatLogicalName);
        File.Delete(filepath);
    }
    /* Test Draw and Set Transparency */
    [Fact]
    public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    {
        //black cat load
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image blackCat = Jpg.Load(filepath);
        ValidateImagePng(blackCat, BlackCatLogicalName);
        blackCat.SetAlphaPercentage(0.5);
        //yellow cat load
        string filepath2 = SaveEmbeddedResourceToFile(SquareCatLogicalName);
        Image yellowCat = Jpg.Load(filepath2);
        ValidateImageJpeg(yellowCat, SquareCatLogicalName);
        yellowCat.Draw(blackCat, 0, 0);
        ValidateImageJpeg(yellowCat, SquareCatLogicalName);
    }

    [Fact]
    public static void WhenAddingAGreyScaleFilterToAJpegGiveAValidGreyScaledImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);

        Image img1 = Jpg.Load(filepath);
        img1.ApplyMatrixMultiplier(ImageExtensions.GreyScaleMatrix);
        ValidateImageJpeg(img1, SquareCatLogicalName);
        Jpg.Write(img1, Path.GetTempPath() + "GreyscaleCat.jpg");
    }
    [Fact]
    public static void WhenAddingAGreyScaleFilterToAPngGiveAValidGreyScaledImage()
    {
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image img1 = Png.Load(filepath);
        img1.ApplyMatrixMultiplier(ImageExtensions.GreyScaleMatrix);
        ValidateImagePng(img1, BlackCatLogicalName);
        Png.Write(img1, Path.GetTempPath() + "GreyscaleCat.png");
    }

    [Fact]
    public static void WhenAddingASepiaFilterToAJpegGiveAValidGreyScaledImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);

        Image img1 = Jpg.Load(filepath);
        img1.ApplyMatrixMultiplier(ImageExtensions.SepiaMatrix);
        ValidateImageJpeg(img1, SquareCatLogicalName);
        Jpg.Write(img1, Path.GetTempPath() + "SepiaCat.jpg");
    }
    [Fact]
    public static void WhenAddingASepiaFilterToAPngGiveAValidGreyScaledImage()
    {
        string filepath = SaveEmbeddedResourceToFile(CuteCatLogicalName);
        Image img1 = Png.Load(filepath);
        img1.ApplyMatrixMultiplier(ImageExtensions.SepiaMatrix);
        ValidateImagePng(img1, CuteCatLogicalName);
        Png.Write(img1, Path.GetTempPath() + "SepiaCat.png");
    }

    [Fact]
    public static void WhenAddingANegativeFilterToAJpegGiveAValidGreyScaledImage()
    {
        string filepath = SaveEmbeddedResourceToFile(SquareCatLogicalName);

        Image img1 = Jpg.Load(filepath);
        img1.ApplyMatrixMultiplier(ImageExtensions.NegativeMatrix);
        ValidateImageJpeg(img1, SquareCatLogicalName);
        Jpg.Write(img1, Path.GetTempPath() + "NegativeCat.jpg");
    }
    [Fact]
    public static void WhenAddingANegativeFilterToAPngGiveAValidGreyScaledImage()
    {
        string filepath = SaveEmbeddedResourceToFile(BlackCatLogicalName);
        Image img1 = Png.Load(filepath);
        img1.ApplyMatrixMultiplier(ImageExtensions.NegativeMatrix);
        ValidateImagePng(img1, BlackCatLogicalName);
        Png.Write(img1, Path.GetTempPath() + "NegativeCat.png");
    }

    /*Tests CircleCrop*/

    //Tests filpath
    //Tests jpg
    [Fact]
    public void WhenCropingAnJpgImageFromFileGiveACorrectCroppedImage()
    {
        //checking with cat image
        string filepath = SaveEmbeddedResourceToFile(JpegCatLogicalName);
        Image avatarImage = Jpg.Load(filepath);
        Image newImage = avatarImage.CircleCrop(0, 0);

    }
    //Tests png
    [Fact]
    public void WhenCropingAnPngImageFromFileGiveACorrectCroppedImage()
    {
        //checking with cat image
        string filepath = SaveEmbeddedResourceToFile(PngCatLogicalName);
        Image avatarImage = Png.Load(filepath);
        Image newImage = avatarImage.CircleCrop(0, 0);
    }

    //Tests stream
    //Tests jpg
    [Fact]
    public void WhenCropingAnJpgImageFromFileStreamACorrectCroppedImage()
    {
        string filepath = SaveEmbeddedResourceToFile(JpegCatLogicalName);
        using (FileStream filestream = new FileStream(filepath, FileMode.Open))
        {
            Image avatarImage = Jpg.Load(filestream);
            Image newImage = avatarImage.CircleCrop(0, 0);
        }

    }

    //Tests png
    [Fact]
    public void WhenCropingAnPngImageFromFileStreamACorrectCroppedImage()
    {
        string filepath = SaveEmbeddedResourceToFile(PngCatLogicalName);
        using (FileStream filestream = new FileStream(filepath, FileMode.Open))
        {
            Image avatarImage = Png.Load(filestream);
            Image newImage = avatarImage.CircleCrop(0, 0);
        }

    }

    /* ------------------Performance Tests-------------------------*/

#if PERFORMANCE_TESTING
    [Fact]
    public static void RunAllPerfTests()
    {
        string filepath = Path.GetTempPath() + "Trial1Results.txt";
        if (File.Exists(filepath)) File.Delete(filepath);
        FileStream fstream = new FileStream(filepath, FileMode.OpenOrCreate);
        streamwriter = new StreamWriter(fstream);
        //set temppaths of files perf test images
        SetTempPathsOfPerfTestFiles();
        runTests(1);
        runTests(10);
        runTests(100);
        runTests(1000);
        runTests(5000);
        streamwriter.Dispose();
        fstream.Dispose();
        //delete perf test images files
        DeletePerfTestFileConstants();
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
        //WriteJpg
        WriteCurrentTest("WriteJpeg", numRuns);
        WriteFileJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteFileJpeg");
        //WritePng
        WriteCurrentTest("WritePng", numRuns);
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
        //WriteJpg
        WriteCurrentTest("WriteJpeg", numRuns);
        WriteStreamJpegPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteStreamJpeg");
        //WritePng
        WriteCurrentTest("WritePng", numRuns);
        WriteStreamPngPerfTest(numRuns);
        WriteStopWatch(stopwatchSingleThread, "WriteStreamPng");

    }

    [Fact]
    public static void SetUpAllPerfTestsWithThreads()
    {
        int numOfTasks = 4;
        string filepath = Path.GetTempPath() + "Trial2Results.txt";
        if (File.Exists(filepath)) File.Delete(filepath);

        //set temp paths of files perf test images
        SetTempPathsOfPerfTestFiles();
        FileStream fstream = new FileStream(filepath, FileMode.OpenOrCreate);
        streamwriter = new StreamWriter(fstream);
        WriteTestHeader(1);
        RunAllPerfTestsWithThreads(numOfTasks, 1);
        WriteTestHeader(10);
        RunAllPerfTestsWithThreads(numOfTasks, 10);
        WriteTestHeader(100);
        RunAllPerfTestsWithThreads(numOfTasks, 100);
        WriteTestHeader(1000);
        RunAllPerfTestsWithThreads(numOfTasks, 1000);
        WriteTestHeader(5000);
        RunAllPerfTestsWithThreads(numOfTasks, 5000);
        streamwriter.Dispose();
        fstream.Dispose();

        //delete temp perf tests images files
        DeletePerfTestFileConstants();

    }

    private static void RunAllPerfTestsWithThreads(int numOfTasks, int numRuns)
    {
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "LoadFileJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "LoadFilePngPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WriteJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WritePngPerfTest");
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
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WriteJpegPerfTest");
        RunOneFuntionWithMultipleTasks(numOfTasks, numRuns, "WritePngPerfTest");
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
        }
        Task.WaitAll(tasks);
        stopwatchMultiThread.Stop();
        WriteStopWatch(stopwatchMultiThread, functionToRun);
        //delete dump dir
    }

    private static void SetTempPathsOfPerfTestFiles()
    {
        jpegDogPath = SaveEmbeddedResourceToFile("JpegDog");
        jpegCatPath = SaveEmbeddedResourceToFile("JpegCat");
        pngDogPath = SaveEmbeddedResourceToFile("PngDog");
        pngCatPath = SaveEmbeddedResourceToFile("PngCat");
    }

    private static void DeletePerfTestFileConstants()
    {
        File.Delete(jpegDogPath);
        File.Delete(jpegCatPath);
        File.Delete(pngDogPath);
        File.Delete(pngCatPath);
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
        TimeSpan elapsedSecs = (sw.Elapsed);
        Console.WriteLine(elapsedSecs);
        Console.WriteLine("");
        streamwriter.WriteLine("Elapsed time for " + currentTest + ": " + elapsedSecs);
        streamwriter.WriteLine("");
        sw.Reset();
    }

    private static void LoadFileJpegPerfTest(int numRuns)
    {
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("LoadFileJpegTest :" + i);

            stopwatchSingleThread.Start();
            Image img = Jpg.Load(jpegCatPath);
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
            Image img = Png.Load(pngCatPath);
            stopwatchSingleThread.Stop();
            img.ReleaseStruct();
        }
    }
    //FIX Write
    private static void WriteFileJpegPerfTest(int numRuns)
    {
        //string dir = Path.GetTempPath();
        Image _thisjpgdog = Jpg.Load(jpegDogPath);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going 
            if (i % 100 == 0)
            {
                Console.WriteLine("WriteJpegTest :" + i);
            }
            stopwatchSingleThread.Start();
            Jpg.Write(_thisjpgdog, Path.ChangeExtension(Path.GetTempFileName(), ".jpg"));
            stopwatchSingleThread.Stop();
        }
        _thisjpgdog.ReleaseStruct();
    }

    //fix write
    private static void WriteFilePngPerfTest(int numRuns)
    {
        Image _thispngdog = Png.Load(pngDogPath);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("WritePngTest :" + i);
            stopwatchSingleThread.Start();
            Png.Write(_thispngdog, Path.ChangeExtension(Path.GetTempFileName(), ".png"));
            stopwatchSingleThread.Stop();
        }
        _thispngdog.ReleaseStruct();
    }

    private static void ResizeJpegPerfTest(int numRuns)
    {
        Image _thisjpgcat = Jpg.Load(jpegCatPath);
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
        Image _thispngcat = Png.Load(pngCatPath);
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
        Image _thisjpgcat = Jpg.Load(jpegCatPath);
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
        Image _thispngcat = Png.Load(pngCatPath);
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
        Image _thisjpgcat = Jpg.Load(jpegCatPath);
        Image _thisjpgdog = Jpg.Load(jpegDogPath);
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
        Image _thispngcat = Png.Load(pngCatPath);
        Image _thispngdog = Png.Load(pngDogPath);
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
        Image _thisjpgcat = Jpg.Load(jpegCatPath);
        Image _thispngdog = Png.Load(pngDogPath);
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
        Image _thisjpgdog = Jpg.Load(jpegDogPath);
        Image _thispngcat = Png.Load(pngCatPath);
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

            using (FileStream filestream = new FileStream(jpegCatPath, FileMode.Open, FileAccess.Read, FileShare.Read))
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
            using (FileStream filestream = new FileStream(pngCatPath, FileMode.Open, FileAccess.Read, FileShare.Read))
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
        Image _thisjpgcat = Jpg.Load(jpegCatPath);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("WriteJpegTest :" + i);

            using (MemoryStream stream = new MemoryStream())
            {
                stopwatchSingleThread.Start();
                Jpg.Write(_thisjpgcat, stream);
                stopwatchSingleThread.Stop();
            }
        }
        _thisjpgcat.ReleaseStruct();
    }

    private static void WriteStreamPngPerfTest(int numRuns)
    {
        Image _thispngcat = Jpg.Load(pngCatPath);
        for (int i = 0; i < numRuns; i++)
        {
            //make sure it's going
            if (i % 100 == 0)
                Console.WriteLine("WritePngTest :" + i);

            using (MemoryStream stream = new MemoryStream())
            {
                stopwatchSingleThread.Start();
                Png.Write(_thispngcat, stream);
                stopwatchSingleThread.Stop();
            }
        }
        _thispngcat.ReleaseStruct();
    }

#endif
}






