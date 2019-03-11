namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using ActionBase;
    using Checks.ContextDocIndex;
    using Checks.Documents;
    using Checks.Documents.ContextDocOnDisk;
    using Common;
    using Log;
    using Repositories;

    #endregion

    [AdaActionPrecondition("ContextDocumentationIndex")]
    [RequiredChecks(
        typeof(ContextDocumentationNotWellFormed),
        typeof(ContextDocumentationInvalid)
    )]
    [ReportsChecks(
        typeof(ContextDocMissingDoc),
        typeof(ContextDocMissingFromIndex),
        typeof(ContextDocMaxCount))]
    public class ContextDocumentIntegrityAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public ContextDocumentIntegrityAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            new AdaSingleQueryAction(GetSubordinateProcessLog(), GetAdaTestLog(), factory, Mapping).Run(
                typeof(ContextDocMissingDoc));
            new AdaSingleQueryAction(GetSubordinateProcessLog(), GetAdaTestLog(), factory, Mapping).Run(
                typeof(ContextDocMissingFromIndex));
            new AdaSingleQueryAction(GetSubordinateProcessLog(), GetAdaTestLog(), factory, Mapping).Run(
                typeof(ContextDocMaxCount));
        }

        #endregion
    }
}