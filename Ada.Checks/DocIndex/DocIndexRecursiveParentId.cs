namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Repositories;

    #endregion

    public class DocIndexRecursiveParentId : AdaAvViolation
    {
        #region  Constructors

        public DocIndexRecursiveParentId(string dId)
            : base("4.C.6_4")
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
            foreach (var recursiveRef in repo.GetRecursiveIds()) yield return new DocIndexRecursiveParentId(recursiveRef);
        }

        #endregion
    }
}