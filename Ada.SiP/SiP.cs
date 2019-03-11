namespace Ada.SiP
{
    #region Namespace Using

    using Repositories;

    #endregion

    public class SiPRepositories
    {
        #region  Fields

        public ArchiveIndexRepo ArchiveIndexRepo; //OK
        public ContextDocumentationRepo ContextDocumentationRepo; //OK
        public DocumentIndexRepo DocumentIndexRepo;
        public AdaFileIndexRepo FileIndexRepo;

        public FullDocumentRepo FullDocumentRepo;
        public AdaStructureRepo StructureRepo;
        public TableContentRepo TableContentRepo;
        public TableContentRepo TableIndexRepo; // OK

        #endregion
    }
}