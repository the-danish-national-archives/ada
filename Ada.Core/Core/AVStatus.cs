namespace Ada.Core
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Linq;
    using Log;
    using Ra.Common.Reflection;

    #endregion

    public class AVStatus
    {
        #region  Constructors

        public AVStatus()
        {
            StructureReadFiles = new TestStatus();
            StructureCheckFiles = new TestStatus();
            CalculateMD5 = new TestStatus();
            ReadFileIndex = new TestStatus();
            CheckMD5 = new TestStatus();
            ArchiveInformation = new TestStatus();
            ContextDocumentationIndex = new TestStatus();
            ContextDocumentation = new TestStatus();
            TableIndex = new TestStatus();
            RewriteXsd = new TestStatus();
            WriteFileIndex = new TestStatus();
            Tables = new TestStatus();
            PrimaryKeys = new TestStatus();
            ForeignKeys = new TestStatus();
            Views = new TestStatus();
            FunctionalDescriptions = new TestStatus();
            DocumentIndex = new TestStatus();
            Documents = new TestStatus();
//            DocSysFileTest = new TestStatus();
            DocRelationTest = new TestStatus();
        }

        #endregion

        #region Properties

        [UILabels("ArkivIndex", "", "", "Arkiv Information")]
        public TestStatus ArchiveInformation { get; }

        [UILabels("Beregning af MD5", "", "", "Struktur og Filer")]
        public TestStatus CalculateMD5 { get; }

        [UILabels("Sammenligning af index/filer", "", "", "Struktur og Filer")]
        public TestStatus CheckMD5 { get; }

        [UILabels("Dokumenter", "", "", "KontekstDokumentation")]
        public TestStatus ContextDocumentation { get; }

        [UILabels("Index", "", "", "KontekstDokumentation")]
        public TestStatus ContextDocumentationIndex { get; }


        public ISynchronizeInvoke Dispatcher { get; set; } = null;


        [UILabels("Test af Dokumentreferencer", "", "", "Sager og Dokumenter")]
        public TestStatus DocRelationTest { get; }

        [UILabels("Index", "", "", "Dokumenter")]
        public TestStatus DocumentIndex { get; }

        [UILabels("Dokumenter", "", "", "Dokumenter")]
        public TestStatus Documents { get; }

        [UILabels("Fremmednøgler", "", "", "Tabeller")]
        public TestStatus ForeignKeys { get; }

        [UILabels("Funktionsbeskrivelser", "", "", "Tabeller")]
        public TestStatus FunctionalDescriptions { get; }

        [UILabels("Primærnøgler", "", "", "Tabeller")]
        public TestStatus PrimaryKeys { get; }

        [UILabels("Indlæsning af FilIndex", "", "", "Struktur og Filer")]
        public TestStatus ReadFileIndex { get; }

        //        [UILabels("Test af Dokument/Sag relation", "", "", "Sager og Dokumenter")]
        //        public TestStatus DocSysFileTest { get; private set; }


        [UILabels("Dannelse af tabelskemaer", "", "", "Genskrivninger")]
        public TestStatus RewriteXsd { get; }

        [UILabels("Check af fil- og mappestruktur", "", "", "Struktur og Filer")]
        public TestStatus StructureCheckFiles { get; }

        [UILabels("Indlæsning af filer", "", "", "Struktur og Filer")]
        public TestStatus StructureReadFiles { get; }

        [UILabels("Index", "", "", "Tabeller")]
        public TestStatus TableIndex { get; }

        [UILabels("Tabelindlæsning", "", "", "Tabeller")]
        public TestStatus Tables { get; }

        [UILabels("Forespørgsler", "", "", "Tabeller")]
        public TestStatus Views { get; }

        [UILabels("Dannelse af nyt filindex", "", "", "Genskrivninger")]
        public TestStatus WriteFileIndex { get; }

        #endregion

        #region

        private TestStatus GetStatusObject(Type t)
        {
            var order = Controller.AllExecutionOrders.FirstOrDefault(p => p.Action == t);

            if (order.Action != t) return null; // unexpected
            return GetTestStatus(this, order.AvStatusName);
        }

        public static TestStatus GetTestStatus(AVStatus status, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;


            return typeof(AVStatus)
                .GetProperty(name).GetValue(status) as TestStatus;
        }

        public void LoggingEvent(object sender, LoggingEventArgs e)
        {
            var currentTest = GetStatusObject(e.Source);
            if (currentTest != null)
                Dispatcher.BeginInvoke((Action<LoggingEventArgs>) (o =>
                    currentTest.ProcessLoggingEvent(o)), new object[] {e});
        }


        public void LoggingEvent(object sender, AdaTestLog_EventHandlerArgs e)
        {
            var currentTest = GetStatusObject(e.Source);
            if (currentTest != null)
                Dispatcher.BeginInvoke((Action<AdaTestLog_EventHandlerArgs>) (o =>
                    currentTest.ProcessLoggingEvent(o)), new object[] {e});
        }

        public void progressEvent(object sender, LoggingEventArgs e)
        {
            var currentTest = GetStatusObject(e.Source);
            if (currentTest != null) currentTest.ProcessLoggingEvent(e);
        }

        public void Reset()
        {
            StructureCheckFiles.Reset();
            StructureReadFiles.Reset();
            ReadFileIndex.Reset();
            CalculateMD5.Reset();
            CheckMD5.Reset();
            ArchiveInformation.Reset();
            ContextDocumentationIndex.Reset();
            ContextDocumentation.Reset();
            DocumentIndex.Reset();
            Documents.Reset();
            TableIndex.Reset();
            RewriteXsd.Reset();
            WriteFileIndex.Reset();
            Tables.Reset();
            PrimaryKeys.Reset();
            ForeignKeys.Reset();
            Views.Reset();
            FunctionalDescriptions.Reset();
//            DocSysFileTest.Reset();
            DocRelationTest.Reset();
        }

        #endregion
    }
}