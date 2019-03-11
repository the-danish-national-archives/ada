namespace Ada.Checks.Documents.DocumentsOnDisk
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using JetBrains.Annotations;
    using Repositories;

    #endregion

    public class DocumentsFileGap : XFileGap
    {
        #region  Constructors

        public DocumentsFileGap(string path)
            : base("4.G_14", path)
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check([NotNull] AdaStructureRepo repo)
        {
            return Check(repo.EnumerateDocFilesOrder(false),
                path => new DocumentsFileGap(path));
        }

        #endregion
    }
}