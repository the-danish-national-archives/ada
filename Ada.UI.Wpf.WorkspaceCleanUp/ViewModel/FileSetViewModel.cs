namespace Ada.UI.Wpf.WorkspaceCleanUp.ViewModel
{
    #region Namespace Using

    using GalaSoft.MvvmLight;
    using Model;

    #endregion

    public class FileSetViewModel : ViewModelBase
    {
        #region  Fields

        private bool _selected;

        #endregion

        #region  Constructors

        public FileSetViewModel(FileSet fileSet)
        {
            FileSet = fileSet;
        }

        #endregion

        #region Properties

        public FileSet FileSet { get; }

        public string Name => FileSet.Name;

        public bool Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public string Size => $"{FileSet.Size / (1024 * 1024.0):N} Mb";

        #endregion

        #region

        public override string ToString()
        {
            return "test";
        }

        #endregion
    }
}