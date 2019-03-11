namespace Ada.UI.Wpf.WorkspaceCleanUp.ViewModel
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Model;
    using Properties;
    using Ra.Common.Repository.NHibernate;
    using Text.Properties;

    #endregion

    public class CleanUpCurrentViewModel : ViewModelBase
    {
        #region  Fields

        private RelayCommand deleteCommand;

        private SingleFileSetManager fileManager;


        private DirectoryInfo workspaceDirectory;

        #endregion

        #region  Constructors

        public CleanUpCurrentViewModel(string avId = null)
        {
            AvId = avId;

            if (avId == null) return;

            UpdateFileSetManager();
        }

        #endregion

        #region Properties

        public string AvId { get; set; }

        public FileSetViewModel Current { get; set; }

        public RelayCommand DeleteCommand => deleteCommand
                                             ?? (deleteCommand = new RelayCommand(
                                                 DeleteItem,
                                                 () => AvId != null));

        public bool Enabled { get; set; } = true;

        #endregion

        #region

        private void DeleteItem()
        {
            if (AvId == null)
                return;

            var dialogRes = MessageBox.Show(
                string.Format(UIText.WillDeleteF, Current.Name, Current.Size),
                UIText.DeleteDatabasesTitle,
                MessageBoxButtons.OKCancel
            );
            if (dialogRes != DialogResult.OK)
                return;

            UnitOfWork.DisposeAll();

            var success = false;
            while (!success)
            {
                var breakOut = 3;
                while (breakOut-- > 0 && (success = fileManager.DeleteItem())) Thread.Sleep(new TimeSpan(0, 0, 0, 0, 500));

                if (success)
                    break;

                dialogRes = MessageBox.Show(
                    string.Format(UIText.UnableToDelete, Current.Name, Current.Size),
                    UIText.DeleteDatabasesTitle,
                    MessageBoxButtons.RetryCancel
                );
                if (dialogRes == DialogResult.Cancel)
                    success = true;
            }

            Environment.Exit(0);
        }

        private void UpdateFileSetManager()
        {
            workspaceDirectory = new DirectoryInfo(Settings.Default.DBCreationFolder);
            fileManager = new SingleFileSetManager(workspaceDirectory, AvId);

            Current = new FileSetViewModel(fileManager.FileSet);
        }

        #endregion
    }
}