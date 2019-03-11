namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using ActionBase;
    using Checks.DocIndex;
    using Checks.Documents;
    using Checks.Documents.DocumentsOnDisk;
    using Common;
    using Log;
    using Repositories;

    #endregion

    [AdaActionPrecondition("DocumentIndex")]
    [RequiredChecks(
        typeof(DocIndexNotWellFormed),
        typeof(DocIndexInvalid)
    )]
    [ReportsChecks(
        typeof(DocIndexNestedDocCount),
        typeof(DocumentsMissingDoc),
        typeof(DocIndexMissingParentId),
        typeof(DocumentsMissingFromIndex),
        typeof(DocIndexRecursiveParentId),
        typeof(DocumentsMaxCount),
        typeof(DocumentsTypeListing),
        typeof(DocIndexUniqueDocIds)
    )]
    public class DocIndexIntegrityCheckAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public DocIndexIntegrityCheckAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var repo = new DocumentIndexRepo(
                factory,
                1000))
            {
                ReportAny(DocIndexNestedDocCount.Create(repo));

                ReportAny(DocIndexMissingParentId.Check(repo));

                ReportAny(DocIndexRecursiveParentId.Check(repo));

                ReportAny(DocIndexUniqueDocIds.Check(repo));
            }

            new AdaSingleQueryAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(), factory, Mapping).Run(typeof(DocumentsMissingDoc));
            new AdaSingleQueryAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(), factory, Mapping).Run(typeof(DocumentsMissingFromIndex));
            new AdaSingleQueryAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(), factory, Mapping).Run(typeof(DocumentsMaxCount));
            new AdaSingleQueryAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(), factory, Mapping).Run(typeof(DocumentsTypeListing));
        }

        #endregion
    }
}