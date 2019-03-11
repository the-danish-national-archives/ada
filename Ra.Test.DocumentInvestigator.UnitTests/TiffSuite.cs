namespace Ra.Test.DocumentInvestigator.UnitTests
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Reflection;
    using Common;
    using log4net;
    using NUnit.Framework;
    using Ra.DocumentInvestigator.AdaAvChecking.AdaReporting;
    using Ra.DocumentInvestigator.AdaAvChecking.Image.Tiff;
    using Utilities;

    #endregion

    [TestFixture]
    public class TiffSuite
    {
        private const int SampleSizeFiles = 10;
        private const int SampleSizePages = 3;
        private const string TestMaterialFolder = "TestDocs";
        private readonly string _basePath = "";

        private readonly ILog _log;

        public TiffSuite()
        {
            LogTools.ConfigureWithConsole();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _basePath = TestHelper.SetBasePath(TestMaterialFolder);
            _log.Debug("ctor");
        }


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _log.Debug("OneTimeSetUp");

            _libWithSampleTiffs = Path.Combine(_basePath, _libWithSampleTiffs);
            CreateSampleFiles(_libWithSampleTiffs, SampleSizeFiles);

            _fileNameBlackAndWhite = Path.Combine(_basePath, _fileNameBlackAndWhite);
            _fileNameFourPage = Path.Combine(_basePath, _fileNameFourPage);
            _fileNameMultiPage = Path.Combine(_basePath, _fileNameMultiPage);
            _fileNameNotTiff = Path.Combine(_basePath, _fileNameNotTiff);

            TestHelper.CreateBlackWhiteTiff(_fileNameBlackAndWhite);
            TestHelper.CreateMultipageTiff(_fileNameFourPage);
            TestHelper.CreateMultipageTiff(_fileNameMultiPage, SampleSizePages);
            File.WriteAllText(_fileNameNotTiff, "This is not a Tifffile, but a textfile");
            _baseFile = new FileInfo(_fileNameFourPage);
        }


        private void CreateSampleFiles(string lib, int size)
        {
            Directory.CreateDirectory(lib);
            _log.Debug("CreateSampleFiles");

            for (var i = 0; i < size; i++)
            {
                var filePath = Path.Combine(
                    lib,
                    Path.GetFileNameWithoutExtension(_fileNameMultiPage) + i + Path.GetExtension(_fileNameMultiPage));
                TestHelper.CreateMultipageTiff(filePath, true);
            }
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _log.Debug("Cleanup");
            try
            {
                File.Delete(_fileNameBlackAndWhite);
                File.Delete(_fileNameFourPage);
                File.Delete(_fileNameMultiPage);
                File.Delete(_fileNameNotTiff);
                TestHelper.EraseDirectory(_libWithSampleTiffs, false);
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private string _fileNameBlackAndWhite = "WriteBlackWhiteTiff.tif";
        private string _fileNameFourPage = "Create4pageTiff.tif";
        private string _fileNameNotTiff = "CreateNotTiff.tif";
        private string _fileNameMultiPage = "CreateMultipageTiff.tif";
        private string _libWithSampleTiffs = "LibWithSampleTiffs";
        private FileInfo _baseFile;

        [TestCase]
        public void TestTiffTesterHelper()
        {
            FileAssert.Exists(_fileNameBlackAndWhite);
        }


        [TestCase]
        public void TestTiffObject()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                Assert.IsNotNull(tiffObject);
            }
        }

        [TestCase]
        public void TestTiffObjectFilename()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                //                Assert.AreEqual(_baseFile.Name, tiffObject.Name);
                Assert.AreEqual(
                    Path.Combine(di.DocumentFolder, di.DocumentId, _baseFile.Name)
                    , tiffObject.Name);
            }
        }

        private static DocInfo GetDummyDocInfo(string filePath)
        {
            var di = new DocInfo
            {
                DocumentFolder = filePath,
                DocumentId = "1",
                MediaId = "0"
            };
            return di;
        }

        [TestCase]
        public void TestTiffObjectPageCount()
        {
            var f = new FileInfo(_fileNameBlackAndWhite);
            Console.WriteLine(f.FullName);
            using (var stream = new BufferedProgressStream(f))
            {
                var di = GetDummyDocInfo(f.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                Assert.AreEqual(tiffObject.PageCount, tiffObject.PageObjects.Length);
            }
        }

        [TestCase]
        public void TestTiffObjectFirstPageBlank()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                Assert.IsFalse(tiffObject.IsFirstPageBlank);
            }
        }

        [TestCase]
        [Category("QuickOne")]
        public void TestAllTiffProperties()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                Assert.IsFalse(tiffObject.IsFirstPageBlank);
                Assert.IsTrue(tiffObject.IsTiff);
                Assert.AreEqual(
                    Path.Combine(di.DocumentFolder, di.DocumentId, _baseFile.Name)
                    , tiffObject.Name);
                Assert.IsFalse(tiffObject.IsPrivateTagsFound);
                Assert.AreEqual(tiffObject.PageCount, tiffObject.PageObjects.Length);
            }
        }


        [TestCase]
        public void TestTiffObjectIsTiff()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                Assert.IsTrue(tiffObject.IsTiff);
            }
        }

        [TestCase]
        public void TestTiffObjectIsNotTiff()
        {
            var f = new FileInfo(_fileNameNotTiff);
            using (var stream = new BufferedProgressStream(f))
            {
                var di = GetDummyDocInfo(f.DirectoryName);
                Assert.False(new TiffObject(stream, di).IsTiff);
            }
        }

        [TestCase]
        public void TestTiffObjectPrivateTagsFound()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);
                var tiffObject = new TiffObject(stream, di);
                Assert.IsFalse(tiffObject.IsPrivateTagsFound);
            }
        }


        [TestCase]
        public void TestTiffObjectLegalCompression()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);

                var tiffObject = new TiffObject(stream, di);
//                Assert.IsTrue(tiffObject.LegalCompression);
                Assert.IsFalse(tiffObject.IsCompressionLegal); // NONE is not a legal compression
            }
        }

        [TestCase]
        public void TestTiffObjectHaveOddDBI()
        {
            using (var stream = new BufferedProgressStream(_baseFile))
            {
                var di = GetDummyDocInfo(_baseFile.DirectoryName);

                var tiffObject = new TiffObject(stream, di);
                Assert.IsFalse(tiffObject.IsAnyOddDBI);
            }
        }


//        [TestCase]
//        [Category("LongRunning")]
////        [Ignore("It takes far to long..")]
//        public void TestTifferyTypeOrig()
//        {
//            foreach (var name in TestHelper.LoadTiffsFromDisk(_libWithSampleTiffs))
//            {
//                _log.Debug(name);
//                var f = new FileInfo(name);
//                using (var stream = new BufferedProgressStream(f))
//                {
//                    var tiffObject = new TiffObject(stream, GetDummyDocInfo(f.DirectoryName));
//                    Assert.IsNotNull(tiffObject);
//                }
//                f = null;
//            }
//        }
    }
}