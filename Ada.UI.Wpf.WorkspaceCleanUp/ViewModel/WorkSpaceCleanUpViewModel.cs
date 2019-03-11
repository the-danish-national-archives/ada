namespace Ada.UI.Wpf.WorkspaceCleanUp.ViewModel
{
    #region Namespace Using

    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Model;
    using Ra.Common.ExtensionMethods;
    using Text.Properties;

    #endregion

    public class WorkspaceCleanUpViewModel : ViewModelBase
    {
        #region  Fields

        private bool? allCheck;

        private bool allCheckNeedsUpdating;

        private RelayCommand deleteCommand;

        private ObservableCollection<FileSetViewModel> file_set_view_models;

        private FileSetManager fileManager;

        private DirectoryInfo workspaceDirectory;

        #endregion

        #region  Constructors

        public WorkspaceCleanUpViewModel()
        {
            DBLocationViewModel = new SetDBLocationViewModel();
            DBLocationViewModel.PropertyChanged += (s, e) => UpdateFileSetManager();


            UpdateFileSetManager();

            allCheck = false;
        }

        #endregion

        #region Properties

        public bool? AllCheck
        {
            get
            {
                if (allCheckNeedsUpdating)
                {
                    var temp = FileSetViewModels.Aggregate(
                        FileSetViewModels.FirstOrDefault()?.Selected,
                        (b, f) => f.Selected == b ? b : null);

                    allCheckNeedsUpdating = false;
                    if (allCheck != temp)
                        Set(ref allCheck, temp);
                }

                return allCheck;
            }
            set
            {
                if (value == null)
                    value = false;
                if (Set(ref allCheck, value))
                    foreach (var fileSetViewModel in FileSetViewModels)
                        fileSetViewModel.Selected = (bool) value;
            }
        }

        public SetDBLocationViewModel DBLocationViewModel { get; }

        /// <summary>
        ///     Gets the MyCommand.
        /// </summary>
        public RelayCommand DeleteCommand => deleteCommand
                                             ?? (deleteCommand = new RelayCommand(
                                                 DeleteItem));

        public ObservableCollection<FileSetViewModel> FileSetViewModels
        {
            get => file_set_view_models;
            private set => Set(ref file_set_view_models, value);
        }

        #endregion

        #region

        private void DeleteItem()
        {
            var internListVM = FileSetViewModels.Where(fileSetViewModel => fileSetViewModel.Selected).ToList();
            var internList = internListVM
                .Select(fileSetViewModel => fileSetViewModel.FileSet).ToList();

            if (!internList.Any())
                return;

            var dialogResult = MessageBox.Show(
                UIText.WillDelete + "\n"
                                  + internListVM.Select(f => $"\t{f.Name} ({f.Size})").SmartToString("\n"),
                UIText.DeleteDatabasesTitle,
                MessageBoxButton.OKCancel);

            if (dialogResult != MessageBoxResult.OK)
                return;


            fileManager.DeleteItems(internList);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateAllCheck();
        }

        private void UpdateAllCheck()
        {
            allCheckNeedsUpdating = true;
            RaisePropertyChanged(nameof(AllCheck));
        }


        private void UpdateFileSetManager()
        {
            workspaceDirectory = new DirectoryInfo(DBLocationViewModel.DBLocation);
            fileManager = new FileSetManager(workspaceDirectory);
            FileSetViewModels = new ObservableCollection<FileSetViewModel>();
            fileManager.FileSets.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Reset) FileSetViewModels.Clear();

                foreach (var item in args.NewItems.AsEnumerableOrEmpty())
                {
                    var fileSet = item as FileSet;
                    if (fileSet == null)
                        continue;

                    FileSetViewModels.Add(new FileSetViewModel(fileSet));
                }

                foreach (var item in args.OldItems.AsEnumerableOrEmpty())
                {
                    var fileSet = item as FileSet;
                    if (fileSet == null)
                        continue;

                    var target = FileSetViewModels.FirstOrDefault(fileSetViewModel => fileSetViewModel.FileSet == fileSet);
                    FileSetViewModels.Remove(target);
                }
            };


            FileSetViewModels.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Reset)
                    foreach (var item in FileSetViewModels)
                    {
                        var model = item;
                        if (model == null)
                            continue;

                        model.PropertyChanged -= Model_PropertyChanged;
                    }

                foreach (var item in args.NewItems.AsEnumerableOrEmpty())
                {
                    var model = item as FileSetViewModel;
                    if (model == null)
                        continue;

                    model.PropertyChanged += Model_PropertyChanged;
                }

                foreach (var item in args.OldItems.AsEnumerableOrEmpty())
                {
                    var model = item as FileSetViewModel;
                    if (model == null)
                        continue;

                    model.PropertyChanged -= Model_PropertyChanged;
                }

                UpdateAllCheck();
            };

            foreach (var fileSet in fileManager.FileSets) FileSetViewModels.Add(new FileSetViewModel(fileSet));
        }

        #endregion
    }
}