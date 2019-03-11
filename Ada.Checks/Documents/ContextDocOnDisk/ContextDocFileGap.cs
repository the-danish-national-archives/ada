namespace Ada.Checks.Documents.ContextDocOnDisk
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using JetBrains.Annotations;
    using Repositories;

    #endregion

    public class ContextDocFileGap : XFileGap
    {
        #region  Constructors

        public ContextDocFileGap(string path)
            : base("4.E_13", path)
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check([NotNull] AdaStructureRepo repo)
        {
            return Check(repo.EnumerateDocFilesOrder(true),
                path => new ContextDocFileGap(path));
        }

        #endregion
    }
}