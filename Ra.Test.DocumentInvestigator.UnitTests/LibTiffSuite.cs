namespace Ra.Test.DocumentInvestigator.UnitTests
{
    #region Namespace Using

    using System.IO;
    using System.Reflection;
    using BitMiracle.LibTiff.Classic;
    using log4net;
    using NUnit.Framework;
    using Utilities;

    #endregion

    [TestFixture]
    public class LibTiffSuite
    {
        private readonly ILog log;

        private string file_name_four_page = "Create4pageTiff.tif";

        private string file_name_multi_page = "CreateMultipageTiff.tif";

        private string file_name_black_and_white = "WriteBlackWhiteTiff.tif";

        private const int SampleSizePages = 3;

        private const string TestMaterialFolder = "TestDocs";

        private readonly string base_path = TestHelper.SetBasePath(TestMaterialFolder);

        public LibTiffSuite()
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("ctor");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            log.Debug("OneTimeSetUp");
            file_name_black_and_white = Path.Combine(base_path, file_name_black_and_white);
            file_name_four_page = Path.Combine(base_path, file_name_four_page);
            file_name_multi_page = Path.Combine(base_path, file_name_multi_page);
            TestHelper.CreateBlackWhiteTiff(file_name_black_and_white);
            TestHelper.CreateMultipageTiff(file_name_four_page);
            TestHelper.CreateMultipageTiff(file_name_multi_page, SampleSizePages);
        }

        [TestCase]
        public void TestTiffInformationPageCountBasic()
        {
            Assert.AreEqual(4, TestHelper.CalculatePageNumber(file_name_four_page));
        }

        [TestCase]
        public void TestTiffInformationPageCountOneMulti()
        {
            Assert.Greater(TestHelper.CalculatePageNumber(file_name_multi_page), 0);
        }

        [TestCase]
        public void TestTiffInformationPageCounting()
        {
            Assert.AreEqual(
                TestHelper.CalculatePageNumber(file_name_multi_page),
                TestHelper.GetPageNumber(file_name_multi_page));
        }

        [TestCase]
        public void TestTifferyPageTimeLibTiff()
        {
            Assert.AreEqual(SampleSizePages, TestHelper.GetPageNumber(file_name_multi_page));
        }

        [TestCase]
        public void TestTiffInformationPageGetOneMulti()
        {
            log.Debug(new FileInfo(file_name_black_and_white).FullName);
            Assert.Greater(TestHelper.GetPageNumber(file_name_multi_page), 0);
        }

        [TestCase]
        public void TestTiffLib()
        {
            using (var image = Tiff.Open(file_name_multi_page, "r"))
            {
                var value = image.GetField(TiffTag.IMAGEWIDTH);
                var width = value[0].ToInt();

                value = image.GetField(TiffTag.IMAGELENGTH);
                var height = value[0].ToInt();

                value = image.GetField(TiffTag.XRESOLUTION);
                var dpiX = value[0].ToFloat();

                value = image.GetField(TiffTag.YRESOLUTION);
                var dpiY = value[0].ToInt();

                value = image.GetField(TiffTag.COMPRESSION);
                var s = value[0].Value.ToString();

                log.Debug($"TIFF properties: Width = {width}, Height = {height}, DPI = {dpiX}x{dpiY}");
                log.Debug($"Compression: {s}");
                StringAssert.AreEqualIgnoringCase("NONE", s);
            }
        }
    }
}