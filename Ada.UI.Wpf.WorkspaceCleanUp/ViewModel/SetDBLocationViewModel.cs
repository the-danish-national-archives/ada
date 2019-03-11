namespace Ada.UI.Wpf.WorkspaceCleanUp.ViewModel
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Windows.Forms;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Properties;

    #endregion

    public class SetDBLocationViewModel : ViewModelBase
    {
        #region  Fields

        private RelayCommand<string> _setDBLocationCommand;

        #endregion

        #region Properties

        public string DBLocation
        {
            get
            {
                var value = Settings.Default.DBCreationFolder;
                if (string.IsNullOrWhiteSpace(value))
                    value = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Statens Arkiver\\ADA");
                return value;
            }
        }

        public RelayCommand<string> SetDBLocationCommand => _setDBLocationCommand ?? (_setDBLocationCommand =
                                                                new RelayCommand<string>(
                                                                    SetDBLocationWithFolderPicker
//                                                         ,
//                                                     p => IsDirectoryPathValid(p)
                                                                ));

        #endregion

        #region

        public void SetDBLocationWithFolderPicker(string path)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = path;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                if (Directory.Exists(path))
                {
                    Settings.Default.DBCreationFolder = path;
                    Settings.Default.Save();
                    RaisePropertyChanged(nameof(DBLocation));
                }
            }
        }

        #endregion

//        // almost stolen from http://stackoverflow.com/questions/422090/in-c-sharp-check-that-filename-is-possibly-valid-not-that-it-exists
//        private bool IsDirectoryPathValid(string path)
//        {
//            System.IO.DirectoryInfo fi = null;
//            try
//            {
//                fi = new System.IO.DirectoryInfo(path);
//            }
//            catch (ArgumentException) { }
//            catch (System.IO.PathTooLongException) { }
//            catch (NotSupportedException) { }
//            if (ReferenceEquals(fi, null))
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }
    }
}