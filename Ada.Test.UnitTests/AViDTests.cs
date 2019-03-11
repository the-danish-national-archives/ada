namespace Ada.Test.UnitTests.ADA.Common
{
    #region Namespace Using

    using NUnit.Framework;
    using Ra.DomainEntities;

    #endregion

    [TestFixture]
    [Category("AViD")]
    public class AViDTests
    {
        [TestCase(@"W:\testSuits\AVID.SA.17252.1", "SA")]
        [TestCase(@".\testSuits\AVID.SA.17252.1", "SA")]
        public void ExtractArchiveCodeTest(string input, string expected)
        {
            // Arrange

            // Act
            var actual = AViD.ExtractArchiveCode(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(@"W:\AVID.SA.17252.1", "17252")]
        [TestCase(".\\testSuits\\AVID.SA.17252.1", "17252")]
        public void ExtractAVSerialTest(string input, string expected)
        {
            // Arrange

            // Act
            var actual = AViD.ExtractAVSerial(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(@"W:\testSuits\AVID.SA.17252.1", "1")]
        [TestCase(@"W:\testSuits\AVID.SA.17252.1_old", null)]
        [TestCase(@".\testSuits\AVID.SA.17252.1", "1")]
        [TestCase(@"W:\testSuits\AVID.SA.17252.1.old", null)]
        [TestCase(@"W:\testSuits\AVID.SA.17252.1.2", null)]
        public void ExtractMediaNumberTest(string input, string expected)
        {
            // Arrange

            // Act
            var actual = AViD.ExtractMediaNumber(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(@"W:\testSuits\AVID.S", null)]
        [TestCase(@".\testSuits\AVID.S", null)]
        public void CreateAVTest(string input, string expected)
        {
            // Arrange

            // Act
            var actual = new AViD(input);

            // Assert
            Assert.AreEqual(expected, actual.AVSerial);
        }
    }
}