#region Header

// Author 
// Created 13

#endregion

namespace Ada.UI.Wpf.StartWindow.ViewModel
{
    #region Namespace Using

    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities;
    using WorkspaceCleanUp.ViewModel;

    #endregion

    public class StartWindowViewModel : ViewModelBase
    {
        #region  Fields

        private string avId;

        private string isSelectionValidError;

        private bool isUpdatingIsSelectionValid;

        private ICommand openCommand;

        private ICommand openFolderSelectionCommand;
        private bool? result;

        #endregion

        #region  Constructors

        public StartWindowViewModel(TestSettings testSettings = null)
        {
            testSettings = testSettings ?? new TestSettings();

            TestSettings = testSettings;

            CleanUp = new WorkspaceCleanUpViewModel();

            DrivesSelectorViewModel = new DrivesSelectorViewModel(testSettings);

            PropertyChanged += (sender, args) => UpdateIsSelectionValid();
            DrivesSelectorViewModel.PropertyChanged += (sender, args) => UpdateIsSelectionValid();
            NotifyCollectionChangedEventHandler drivesOnCollectionChanged = (sender, args) =>
            {
                foreach (var n in args.NewItems.AsEnumerableOrEmpty().Cast<DriveStatusViewModel>()) n.PropertyChanged += (o, eventArgs) => UpdateIsSelectionValid();

                UpdateIsSelectionValid();
            };
            DrivesSelectorViewModel.Drives.CollectionChanged += drivesOnCollectionChanged;
            drivesOnCollectionChanged.Invoke(
                DrivesSelectorViewModel,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, DrivesSelectorViewModel.Drives));
            UpdateIsSelectionValid();
        }

        #endregion

        #region Properties

        public string AvId
        {
            get => avId;

            set => Set(ref avId, value);
        }

        public WorkspaceCleanUpViewModel CleanUp { get; }

        public DrivesSelectorViewModel DrivesSelectorViewModel { get; }

        public string IsSelectionValidError
        {
            get => isSelectionValidError;

            private set => Set(ref isSelectionValidError, value);
        }

        public ICommand OpenCommand
        {
            get
            {
                return openCommand ?? (openCommand = new RelayCommand(
                           () =>
                               Result = IsSelectionValidError == null ? (bool?) true : null,
                           () => IsSelectionValidError == null));
            }
        }

        public ICommand OpenFolderSelectionCommand =>
            openFolderSelectionCommand ?? (openFolderSelectionCommand = new RelayCommand(
                OpenFolderSelection));

        public bool? Result
        {
            get => result;

            set => Set(ref result, value);
        }

        public TestSettings TestSettings { get; }

        #endregion

        #region

        private string GetValidationError()
        {
            if (!TestSettings.Drives.GetActiveDrives().Any()) return "Der er ikke valgt nogle aktive drev ";

            var id = new AViD(AvId);
            var mediaDirectories = AVMapping.GetMediaDirectories(id, TestSettings.Drives.GetActiveDrives());
            if (mediaDirectories.Count < 1) return "Afleveringen eksisterer ikke";

            return null;
        }

        private void OpenFolderSelection()
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = AvId;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;
                if (Directory.Exists(path))
                {
                    // update drive via viewModel for changes to affect view (TestSettings.Drives is not notifieding itself)
                    var root = Path.GetPathRoot(path);
                    var driveStatus = DrivesSelectorViewModel.Drives.FirstOrDefault(d => d.Drive.StartsWith(root));
                    if (driveStatus != null) driveStatus.Status = true;

                    AvId = Path.GetFileName(path);
                }
            }
        }

        public void UpdateIsSelectionValid()
        {
            if (isUpdatingIsSelectionValid)
                return;

            isUpdatingIsSelectionValid = true;
            IsSelectionValidError = GetValidationError();
            isUpdatingIsSelectionValid = false;
        }

        #endregion
    }
}