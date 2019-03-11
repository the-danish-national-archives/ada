#region Header

// Author 
// Created 21

#endregion

namespace Ada.Main
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CommandLine;
    using Common;
    using Core;
    using log4net;
    using Log;
    using Properties;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    public class Headless
    {
        #region Static

        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region

        private static void AdaTestLog_LoggingEvent
            (object sender, AdaTestLog_EventHandlerArgs e, bool verbose, bool quiet)
        {
            if (!verbose)
                return;

            if (quiet && e.Event.Severity != 1)
                return;

            Console.WriteLine(e.Event.FormattedText);
            WriteToLog(e.Event.FormattedText);
        }

        public static int HeadlessMain(string[] args)
        {
//            if (Debugger.IsAttached == false) Debugger.Launch();

            // Needed to write to the console, when compiled as a windows application
            if (!ConsoleAttacher.AttachParrentProcessToConsole())
                WriteToLog("Failed to attach parent proces to console");

            var options = new Options();

            var isParsed = Parser.Default.ParseArguments(args, options);

            InitializationMethods.PrepareLogging(logFolder: options.LogFileFolder, splitInTwo: true);

            if (!isParsed)
            {
                WriteToLog("Fejl i parametre");

                Console.WriteLine("Fejl i parametre");
                Console.WriteLine(options.GetUsage());

                WriteToLog("Ada exits!");
                Console.WriteLine("Ada exits!");
                return Program.ErrorCode;
            }

            if (!InitializationMethods.CheckExistance(options.InputFile)
                || !InitializationMethods.CheckExistance(options.SettingsFile, true))
            {
                WriteToLog("Fejl i parametre");
                WriteToLog("AV mappe eller settings.xml fil findes ikke!");

                Console.WriteLine("Fejl i parametre");
                Console.WriteLine("AV mappe eller settings.xml fil findes ikke!");

                WriteToLog("Ada exits!");
                return Program.ErrorCode;
            }

            if (options.OtherRoots != null)
            {
                var rootPaths = options.OtherRoots.Split(Path.PathSeparator);

                foreach (var path in rootPaths)
                    if (!InitializationMethods.CheckExistance(path))
                    {
                        Console.WriteLine($@"Unable to locate the root path {path}");

                        WriteToLog("Ada exits!");
                        return Program.ErrorCode;
                    }
            }

            if (!InitializationMethods.CheckExistance(options.LogFileFolder))
                Log.Info("Log destination folder is not specified!");

            if (!InitializationMethods.CheckExistance(options.OutputFile)) Log.Info("Output file not specified!");

            Log.Debug("Start app mode: ReadHeadLess");

            Log.Debug("Parsed arguments > inputfile " + options.InputFile);
            Log.Debug("Parsed arguments > otherRoots " + options.OtherRoots);
            Log.Debug("Parsed arguments > settingsfile " + options.SettingsFile);
            Log.Debug("Parsed arguments > outputfile " + options.OutputFile);
            Log.Debug("Parsed arguments > logfolder " + options.LogFileFolder);

            return RunHeadLess(options);
        }

        private static int RunHeadLess(Options options)
        {
            var versionText = $"ADA v {Application.ProductVersion}";
            Console.WriteLine(versionText);
            try
            {
                // set initial database directory
                InitializationMethods.SetDbCreationDirectory(options.OutputFile);
                Log.Info($"DbCreationFolder set to {Settings.Default.DBCreationFolder}");

                if (!InitializationMethods.CheckExistance(options.SettingsFile, true))
                {
                    WriteToLog("Ada Exit options.SettingsFile is null or not existing");
                    WriteToLog("RunHeadles Exit!");
                    return Program.ErrorCode;
                }

                var AVString = new DirectoryInfo(options.InputFile).Name;
                var settings = TestSettings.Deserialize(new FileInfo(options.SettingsFile));

                settings.InputDictionaries =
                    Directory.GetParent(options.InputFile)
                        .FullName.Yield()
                        .Union(options.OtherRoots?.Split(Path.PathSeparator) ?? Enumerable.Empty<string>())
                        .ToArray();

                if (options.OtherRoots != null)
                {
                    settings.Drives.DriveList.Clear();

                    settings.Drives.DriveList.AddRange(settings.InputDictionaries.Select(s =>
                        new DriveStatus {Drive = Path.GetPathRoot(s), Status = true}));
                }

                //PrepareDrives(settings, options.InputFile);


                WriteToLog("Reading settings from file > Done");

                var ID = new AViD(AVString);

                WriteToLog("AVID: " + ID.FullID);


                {
                    var path = new DirectoryInfo(Settings.Default.DBCreationFolder);
                    if (!AdaBaseUowFactory.DataBaseExists(ID, "log", path) || !options.Continue)
                    {
                        var t = Task.Run(
                            () =>
                            {
                                using (var testFactory = new AdaTestUowFactory(ID, "test", path))
                                using (var logFactory = new AdaLogUowFactory(ID, "log", path))
                                using (var avFactory = new AdaAvUowFactory(ID, "av", path))
                                {
                                    testFactory.DeleteDataBase();
                                    testFactory.CreateDataBase();
                                    using (var uow = (UnitOfWork) testFactory.GetUnitOfWork())
                                    {
                                        using (var cmd = uow.Session.Connection.CreateCommand())
                                        {
                                            cmd.CommandText = Resources.DbCreate;
                                            cmd.ExecuteNonQuery();
                                        }
                                    }

                                    logFactory.DeleteDataBase();
                                    logFactory.CreateDataBase();
                                    var errorTexts =
                                        new FileInfo(
                                            Path.Combine(
                                                Path.GetDirectoryName(Application.ExecutablePath) ?? "",
                                                "errortexts.xml"));
                                    AdaTestLog.LoadErrorTypesFromFile(errorTexts, logFactory);
                                    avFactory.DeleteDataBase();
                                    avFactory.CreateDataBase();
                                }
                            });
                        t.Wait();
                    }
                }
                Func<AdaTestLog_EventHandler> adaTestLoggingEventFactory =
                    () => (sender, e) => AdaTestLog_LoggingEvent(sender, e, options.Verbose, options.Quiet);
                AdaTestLog.LoggingEvent += adaTestLoggingEventFactory();


                Controller.DoFullTest(ID, settings);

                return Program.SuccessCode;
            }
            catch (Exception e)
            {
                //Log.Error(e);
                WriteToLog($"{nameof(RunHeadLess)} failed with exception:  {e.Message}");

                // Console.ReadKey();
                return Program.ErrorCode;
            }
        }

        public static void WriteToLog(string message)
        {
            Log.Debug(message);

            Log.Info(message);
        }

        #endregion
    }
}