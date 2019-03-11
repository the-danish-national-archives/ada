namespace Ada.Checks.DocumentsContent
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Text;
    using ChecksBase;
    using Ra.DomainEntities.Documents;

    #endregion

    [AdaAvCheckToResultsList(nameof(DocIds))]
    public class DocumentsTiffPrivateTags : AdaAvViolation
    {
        #region  Constructors

        public DocumentsTiffPrivateTags(string documents, int count, string docIds)
            : base("5.E_7")
        {
            Folder = documents;
            Count = count;
            DocIds = docIds;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTag(Hidden = true)]
        public string DocIds { get; set; }

        [AdaAvCheckNotificationTag]
        public string Folder { get; set; }

        #endregion

        #region Nested type: DocumentsTiffPrivateTagsSummary

        public class DocumentsTiffPrivateTagsSummary
        {
            #region  Fields

            private readonly StringBuilder docIds = new StringBuilder();

            private int count;

            #endregion

            #region  Constructors

            public DocumentsTiffPrivateTagsSummary(string documents)
            {
                Folder = documents;
            }

            #endregion

            #region Properties

            public string Folder { get; }

            #endregion

            #region

            public void Check(DocIndexEntry di)
            {
                docIds.Append(di.DocumentId);
                docIds.Append(';');
                count++;
            }

            public IEnumerable<AdaAvViolation> Report()
            {
                if (count > 0)
                    yield return new DocumentsTiffPrivateTags(Folder, count, docIds.ToString());
            }

            #endregion
        }

        #endregion

//        public DocumentsTiffPrivateTags(DocIndexEntry docInfo, string path)
//            : base("5.E_7")
//        {
//            Path = path;
//            DocumentID = docInfo.DocumentId;
//        }
//
//        [AdaAvCheckNotificationTag]
//        public string DocumentID { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public string Path { get; set; }
    }
}