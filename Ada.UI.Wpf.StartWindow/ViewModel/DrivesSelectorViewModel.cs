#region Header

// Author 
// Created 13

#endregion

namespace Ada.UI.Wpf.StartWindow.ViewModel
{
    #region Namespace Using

    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    #endregion

    public class DrivesSelectorViewModel : ViewModelBase
    {
        #region  Fields

        private readonly TestSettings testSettings;

        private ICommand updateDriveListCommand;

        #endregion

        #region  Constructors

        public DrivesSelectorViewModel()
            : this(null)
        {
        }

        public DrivesSelectorViewModel(TestSettings testSettings)
        {
            testSettings = testSettings ?? new TestSettings();
            this.testSettings = testSettings;
            if (testSettings.Drives.DriveList.Count == 0)
                testSettings.Drives.UpdateDrives();
            Drives =
                new ObservableCollection<DriveStatusViewModel>(
                    testSettings.Drives.DriveList.Select(d => new DriveStatusViewModel(d)));
        }

        #endregion

        #region Properties

        public ObservableCollection<DriveStatusViewModel> Drives { get; }

        public ICommand UpdateDriveListCommand
        {
            get
            {
                return updateDriveListCommand ??
                       (updateDriveListCommand
                           = new RelayCommand(
                               () =>
                               {
                                   testSettings.Drives.UpdateDrives(
                                   );
                                   UpdateDrives();
                               }));
            }
        }

        #endregion

        #region

        private void UpdateDrives()
        {
            Drives.Clear();
            testSettings.Drives.DriveList.ForEach(d => Drives.Add(new DriveStatusViewModel(d)));
        }

        #endregion
    }

    public class DriveStatusViewModel : INotifyPropertyChanged
    {
        #region  Fields

        private readonly DriveStatus status;

        #endregion

        #region  Constructors

        public DriveStatusViewModel(DriveStatus driveStatus)
        {
            status = driveStatus;
        }

        #endregion

        #region Properties

        public string Drive => status.Drive;

        public bool Status
        {
            get => status.Status;

            set
            {
                if (status.Status == value)
                    return;
                status.Status = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}