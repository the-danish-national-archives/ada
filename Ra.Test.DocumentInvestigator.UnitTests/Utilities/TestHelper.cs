namespace Ra.Test.DocumentInvestigator.UnitTests.Utilities
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using BitMiracle.LibTiff.Classic;
    using log4net;
    using Ra.DocumentInvestigator;

    #endregion

    public static class TestHelper
    {
        #region Static

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region

        public static int CalculatePageNumber(string fileName)
        {
            using (var image = Tiff.Open(fileName, "r"))
            {
                var pageCount = 0;
                do
                {
                    ++pageCount;
                } while (image.ReadDirectory());

                return pageCount;
            }
        }

        public static void CreateBlackWhiteTiff(string fileName)
        {
            Log.Debug($"Creating '{fileName}' as tilfile");

            const int Width = 100;
            const int Height = 150;

            Tiff.SetErrorHandler(new KeepQuietWarning());
            using (var output = Tiff.Open(fileName, "w"))
            {
                output.SetField(TiffTag.IMAGEWIDTH, Width);

                output.SetField(TiffTag.IMAGELENGTH, Height);
                output.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                output.SetField(TiffTag.BITSPERSAMPLE, 8);
                output.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                output.SetField(TiffTag.ROWSPERSTRIP, Height);
                output.SetField(TiffTag.XRESOLUTION, 88.0);
                output.SetField(TiffTag.YRESOLUTION, 88.0);
                output.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.INCH);
                output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                output.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                output.SetField(TiffTag.COMPRESSION, Compression.NONE);
                output.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

                var random = new Random();
                for (var i = 0; i < Height; ++i)
                {
                    var buf = new byte[Width];
                    for (var j = 0; j < Width; ++j)
                        buf[j] = (byte) random.Next(255);

                    output.WriteScanline(buf, i);
                }
            }
        }

        public static void CreateMultipageTiff(string fileName)
        {
            const int DefaultPageNo = 4;
            CreateMultipageTiff(fileName, DefaultPageNo);
        }

        public static void CreateMultipageTiff(string fileName, bool randomNumberOfPages)
        {
            var numberOfPages = 4;
            if (randomNumberOfPages)
                numberOfPages = new Random().Next(1, 1000);
            CreateMultipageTiff(fileName, numberOfPages);
        }

        public static void CreateMultipageTiff(string fileName, int pages)
        {
            Log.Debug($"Creating '{fileName}' as tilfile with {pages} subpages");

            var numberOfPages = pages;

            const int Width = 256;
            const int Height = 256;
            const int SamplesPerPixel = 1;
            const int BitsPerSample = 8;
            Tiff.SetErrorHandler(new KeepQuietWarning());

            var firstPageBuffer = new byte[Height][];
            for (var j = 0; j < Height; j++)
            {
                firstPageBuffer[j] = new byte[Width];
                for (var i = 0; i < Width; i++)
                    firstPageBuffer[j][i] = (byte) (j * i);
            }

            using (var output = Tiff.Open(fileName, "w"))
            {
                for (var page = 0; page < numberOfPages; ++page)
                {
                    output.SetField(TiffTag.IMAGEWIDTH, Width / SamplesPerPixel);
                    output.SetField(TiffTag.SAMPLESPERPIXEL, SamplesPerPixel);
                    output.SetField(TiffTag.BITSPERSAMPLE, BitsPerSample);
                    output.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                    output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);

                    output.SetField(
                        TiffTag.PHOTOMETRIC,
                        page % 2 == 0 ? Photometric.MINISBLACK : Photometric.MINISWHITE);

                    output.SetField(TiffTag.ROWSPERSTRIP, output.DefaultStripSize(0));
                    output.SetField(TiffTag.XRESOLUTION, 100.0);
                    output.SetField(TiffTag.YRESOLUTION, 100.0);
                    output.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.INCH);

                    // specify that it's a page within the multipage file
                    output.SetField(TiffTag.SUBFILETYPE, FileType.PAGE);
                    // specify the page number
                    output.SetField(TiffTag.PAGENUMBER, page, numberOfPages);

                    for (var j = 0; j < Height; ++j)
                        output.WriteScanline(firstPageBuffer[j], j);

                    output.WriteDirectory();
                }
            }
        }

        public static void EraseDirectory(string path, bool recursive)
        {
            if (recursive)
            {
                var subfolders = Directory.EnumerateDirectories(path);
                foreach (var s in subfolders)
                    EraseDirectory(s, true);
            }

            var files = Directory.EnumerateFiles(path);
            foreach (var f in files)
            {
                var attr = File.GetAttributes(f);

                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    File.SetAttributes(f, attr ^ FileAttributes.ReadOnly);

                File.Delete(f);
            }

            Directory.Delete(path);
        }

        public static int GetPageNumber(string fileName)
        {
            using (var image = Tiff.Open(fileName, "r"))
            {
                return image.NumberOfDirectories();
            }
        }

        public static IEnumerable<string> LoadTiffsFromDisk(string pathToTiffs)
        {
            var targetDir = new DirectoryInfo(pathToTiffs).EnumerateFiles("*.tif*", SearchOption.AllDirectories);
            foreach (var f in targetDir)
                yield return f.FullName;
        }

        public static string SetBasePath(string folder)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (basePath != null)
            {
                basePath = basePath.Replace("file:\\", "");
                basePath = Path.Combine(basePath, folder);
                if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                Log.Debug($"basepath is now '{basePath}'");
            }
            else
            {
                Log.Warn("basepath is null");
            }

            return basePath;
        }

        #endregion
    }
}