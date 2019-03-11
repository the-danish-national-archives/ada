namespace Ada.UI.Winforms
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using Properties;
    using Wpf.StartWindow.ViewModel;
    using WPFLocalizeExtension.Engine;
    using Application = System.Windows.Forms.Application;

    #endregion

    public class GuiRunner
    {
        #region

        public static void Run(string avIdPath)
        {
            LocalizeDictionary.Instance.Culture = Thread.CurrentThread.CurrentCulture;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Make sure the WPF Application object is created with the correct ShutdownMode
            if (System.Windows.Application.Current == null) new System.Windows.Application {ShutdownMode = ShutdownMode.OnExplicitShutdown};

            var startWindowViewModel = new StartWindowViewModel();


            //Sætter AV textbox til default at være "AVID.SA."
            startWindowViewModel.AvId = Settings.Default.AViD_Prefix;

            if (avIdPath != null)
            {
                var root = Path.GetPathRoot(avIdPath);
//                startWindowViewModel.TestSettings.Drives.UpdateDrives();
                var driveStatus = startWindowViewModel.TestSettings.Drives.DriveList.FirstOrDefault(d => d.Drive.StartsWith(root));
                if (driveStatus != null) driveStatus.Status = true;

                startWindowViewModel.AvId = Path.GetFileName(avIdPath);
            }

//#if DEBUG
//            foreach (var driveStatus in startWindowViewModel.TestSettings.Drives.DriveList)
//            {
//                driveStatus.Status = true;
//            }
//            // Text after stuteses, PropertyChanged affecting the view
//            startWindowViewModel.AvId += "17300";
//
//
//            // Closes the window
////            startWindowViewModel.Result = true;
//#endif

//            var startWindow = new StartWindow
//                                  {
//                                      DataContext = startWindowViewModel
//                                  };
//
//
////            startWindow.ShowDialog();
//
//            if (startWindowViewModel.Result != true)
//                return;

            var mainForm = new MainForm(startWindowViewModel.TestSettings, startWindowViewModel.AvId);


            Application.Run(mainForm);

            Application.Exit();
            System.Windows.Application.Current?.Shutdown();
            Environment.Exit(0); // TODO Hack? Needed to exit when fx show table properties is open when clicking x on main window
        }

        #endregion
    }
}