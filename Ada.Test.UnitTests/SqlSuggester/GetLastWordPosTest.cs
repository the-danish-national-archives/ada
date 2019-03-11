namespace Ada.Test.UnitTests.SqlSuggester
{
    #region Namespace Using

    using NUnit.Framework;
    using UI.Wpf.TableIndexViewer.DataTableViewer.SqlEditor;

    #endregion

    [TestFixture(Category = "SqlSuggester")]
    public class GetLastWordPosTest
    {
        [TestCase("TABEL.", "TABEL")]
        [TestCase("\"DOKTABEL\".", "DOKTABEL")]
        [TestCase(".", "")]
        [TestCase("TABEL. ", "TABEL")] // dot space
        [TestCase("\"DOKTABEL\". ", "DOKTABEL")]
        [TestCase(". ", "")]
        [TestCase("TABEL .", "TABEL")] // space dot
        [TestCase("\"DOKTABEL\" .", "DOKTABEL")]
        [TestCase(" .", "")]
        [TestCase("TABEL", "TABEL")] // no dots
        [TestCase("\"DOKTABEL\"", "DOKTABEL")]
        [TestCase("", "")]
        [TestCase("TABEL\n.", "TABEL")] // newline dot
        [TestCase("\"DOKTABEL\"\n.", "DOKTABEL")]
        [TestCase("\n.", "")]
        [TestCase("FISK TABEL\n.", "TABEL")] // fisk first
        [TestCase("FISK \"DOKTABEL\"\n.", "DOKTABEL")]
        [TestCase("FISK \n.", "FISK")]
        [TestCase("INDKSTRM.", "INDKSTRM")]
        public void NotTooMany(string text, string expectedSubstring)
        {
            var (start, end) = SqlSuggester.GetLastWordPos(text);

            Assert.AreEqual(expectedSubstring, text.Substring(start, end - start));
            Assert.AreEqual(expectedSubstring, SqlSuggester.GetLastWord(text));
        }
    }
}