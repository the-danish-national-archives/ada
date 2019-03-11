namespace Ada.UI.Wpf.WorkspaceCleanUp.Model
{
    #region Namespace Using

    using System.IO;

    #endregion

    public class SingleFileSetManager
    {
        #region  Fields

        private readonly string nameFilter;

        #endregion

        #region  Constructors

        public SingleFileSetManager(DirectoryInfo dirInfo, string nameFilter)
        {
            this.nameFilter = nameFilter;
            this.dirInfo = dirInfo;

            Aggregate();
        }

        #endregion

        #region Properties

        private DirectoryInfo dirInfo { get; }

        public FileSet FileSet { get; private set; }

        #endregion

        #region

        private void Aggregate()
        {
            if (!dirInfo.Exists)
                return;

            FileSet = new FileSet(nameFilter, dirInfo.EnumerateFiles(nameFilter + "*"));
        }

        public bool DeleteItem()
        {
            var res = FileSet.DeleteItems();

            Aggregate();

            return res;
        }

        #endregion
    }
}