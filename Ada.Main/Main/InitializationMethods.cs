namespace Ada
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Reflection;
    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Filter;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;
    using Properties;

    #endregion

    public class InitializationMethods
    {
        #region Static

        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region

        public static bool CheckExistance(string path, bool isFile = false)
        {
            if (path == null) return false;
            try
            {
                return isFile ? File.Exists(path) : Directory.Exists(path);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void PrepareLogging(bool splitInTwo = false, string logFolder = null)
        {
            SetLogfolder(logFolder);

            var hierarchy = (Hierarchy) LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders(); /*Remove any other appenders*/


            var pl = new PatternLayout
            {
                ConversionPattern =
                    !string.IsNullOrEmpty(Settings.Default.LogConversionPattern)
                        ? Settings.Default.LogConversionPattern
                        : "%d [%2%t] %-5p [%-10c] %m%n%n"
            };
            pl.ActivateOptions();

            var fileAppender = new FileAppender
            {
                Name = "DebugAppender",
                AppendToFile = false,
                LockingModel = new FileAppender.MinimalLock(),
                File = Path.Combine(Settings.Default.LogFolder, "ada_debug_log.txt"),
                Layout = pl
            };

            if (splitInTwo)
            {
                var debugLevelFilter = new LevelMatchFilter {LevelToMatch = Level.Info, AcceptOnMatch = false};
                debugLevelFilter.ActivateOptions();
                fileAppender.AddFilter(debugLevelFilter);

                var infolevelFilter = new LevelMatchFilter {LevelToMatch = Level.Debug, AcceptOnMatch = false};
                infolevelFilter.ActivateOptions();

                var infoFileAppender = new FileAppender
                {
                    Name = "InfoAppender",
                    AppendToFile = true,
                    LockingModel = new FileAppender.MinimalLock(),
                    File =
                        Path.Combine(
                            Settings.Default.LogFolder,
                            "ada_info_log.txt"),
                    Layout = pl
                };
                infoFileAppender.AddFilter(infolevelFilter);
                infoFileAppender.ActivateOptions();

                BasicConfigurator.Configure(infoFileAppender);

                var debugConsoleAppender = new ConsoleAppender {Name = "debugConsoleAppender", Layout = pl, Threshold = Level.All};
//                debugConsoleAppender.AddFilter(debugLevelFilter);
                debugConsoleAppender.ActivateOptions();

                BasicConfigurator.Configure(debugConsoleAppender);
            }

            fileAppender.ActivateOptions();
            BasicConfigurator.Configure(fileAppender);

            hierarchy.Root.Level = Settings.Default.DebugLog ? Level.All : Level.Info;


            // quiet down NHibernate (also seems to slow data accesss and/or startup down (a lot)) 
            var NHibernateLogger = hierarchy.GetLogger("NHibernate") as Logger;
            NHibernateLogger.Level = Level.Warn;
            var NHibernateSQLLogger = hierarchy.GetLogger("NHibernate.SQL") as Logger;
            NHibernateSQLLogger.Level = Level.Warn;

            WriteToLog("LoggingFolder: " + fileAppender.File);
            WriteToLog("Prepare logging done!");
        }

        /// <summary>
        ///     Sets Settings.Default.DBCreationFolder, if either path is specified or the folder in the value is not an exsisting
        ///     folder.
        ///     And ensures the directory exists.
        /// </summary>
        /// <param name="path">
        ///     A path to a dbCreationFolder folder, or null to use default.
        /// </param>
        public static bool SetDbCreationDirectory(string path = null)
        {
            var success = false;
            var creationFolder = string.Empty;

            if (path != null)
            {
                creationFolder = path;
                if (!CheckExistance(creationFolder))
                    try
                    {
                        Directory.CreateDirectory(creationFolder);

                        WriteToLog("UpdateDbCreationDirectory > DbCreation directory : " + creationFolder);

                        success = true;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(
                            "UpdateDbCreationDirectory > DbCreation directory can not be created!!\nException: \n"
                            + exc.Message);

                        WriteToLog(
                            "UpdateDbCreationDirectory > DbCreation directory can not be created!!\nException: \n"
                            + exc.Message);

                        success = false;
                    }
                else
                    success = true;
            }

            if (!success)
                try
                {
                    creationFolder = Settings.Default.DBCreationFolder;
                    if (!CheckExistance(creationFolder))
                    {
                        creationFolder =
                            Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                "Statens Arkiver\\ADA");

                        if (!Directory.Exists(creationFolder)) Directory.CreateDirectory(creationFolder);
                    }

                    success = true;
                }
                catch (Exception exc)
                {
                    WriteToLog("Setting database creation direcory failed with exception: " + exc.Message);
                    success = false;
                }

            if (success) Settings.Default.DBCreationFolder = creationFolder;

            return success;
        }

        /// <summary>
        ///     Sets Properties.Settings.Default.LogFolder, if either logFolderPath is specified or the folder in the value is not
        ///     an exsisting folder.
        ///     And ensures the directory exists.
        /// </summary>
        /// <param name="logFolderPath">
        ///     A path to a log folder, or null to use default.
        /// </param>
        private static void SetLogfolder(string logFolderPath = null)
        {
            if (string.IsNullOrEmpty(logFolderPath))
            {
                if (!CheckExistance(Settings.Default.LogFolder))
                    Settings.Default.LogFolder =
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "Statens Arkiver\\ADA");
            }
            else
            {
                Settings.Default.LogFolder = logFolderPath;
            }

            if (!Directory.Exists(Settings.Default.LogFolder)) Directory.CreateDirectory(Settings.Default.LogFolder);
        }

        private static void WriteToLog(string message)
        {
            Log.Debug(message);

            Log.Info(message);
        }

        #endregion
    }
}