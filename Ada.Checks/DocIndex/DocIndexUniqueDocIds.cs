namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Repositories;

    #endregion

    public class DocIndexUniqueDocIds : AdaAvViolation
    {
        #region  Constructors

        public DocIndexUniqueDocIds(string dId)
            : base("4.C.6_7")
        {
            dID = dId;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string dID { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(DocumentIndexRepo repo)
        {
            foreach (var duplicateDocId in repo.GetDuplicateDocIds()) yield return new DocIndexUniqueDocIds(duplicateDocId);
        }

        #endregion
    }
}