namespace Ada.UI.Wpf.WorkspaceCleanUp.Model
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Ra.DomainEntities;

    #endregion

    public class FileSetManager
    {
        #region  Constructors

        public FileSetManager(DirectoryInfo dirInfo)
        {
            this.dirInfo = dirInfo;

            FileSets = new ObservableCollection<FileSet>();

            Aggregate();
        }

        #endregion

        #region Properties

        private DirectoryInfo dirInfo { get; }

        public ObservableCollection<FileSet> FileSets { get; }

        #endregion

        #region

        private void Aggregate()
        {
            FileSets.Clear();

            if (!dirInfo.Exists)
                return;

            foreach (var enumerateFile in dirInfo.EnumerateFiles("AVID.*")
                .GroupBy(e => $"AVID.{AViD.ExtractArchiveCode(e.Name)}.{AViD.ExtractAVSerial(e.Name)}"))
                FileSets.Add(new FileSet(enumerateFile.Key, enumerateFile));
        }

        public void DeleteItems(List<FileSet> listOfFileSets)
        {
            foreach (var fileSet in listOfFileSets)
                if (fileSet.DeleteItems())
                    FileSets.Remove(fileSet);
            Aggregate();
        }

        #endregion
    }
}