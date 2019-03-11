namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Repositories;

    #endregion

    public class DocIndexNestedDocCount : AdaAvCheckNotification
    {
        #region  Constructors

        public DocIndexNestedDocCount(long docCount, long nestedDocCount)
            : base("4.C.6_1")
        {
            DocCount = docCount;
            Count = nestedDocCount;
            Percentage = (float) nestedDocCount / docCount * 100;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag(Name = "NestedDocCount")]
        public long Count { get; set; }

        [AdaAvCheckNotificationTag]
        public long DocCount { get; set; }

        [AdaAvCheckNotificationTagAsPercentage]
        public float Percentage { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Create(DocumentIndexRepo repo)
        {
            var docCount = repo.TotalDocumentCount();
            var nestedDocCount = repo.NestedDocumentCount();


            yield return new DocIndexNestedDocCount(docCount, nestedDocCount);
        }

        #endregion
    }
}