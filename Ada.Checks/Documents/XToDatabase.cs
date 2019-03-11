namespace Ada.Checks.Documents
{
    public abstract class XToDiskContent
    {
        #region  Constructors

        #endregion

        #region Properties

        public abstract string CollectionsOnDisk { get; }
        public abstract string ContentOnDisk { get; }

        public abstract string Documents { get; }
        public abstract string FolderContent { get; }
        public abstract string FolderName { get; }

        #endregion
    }
}