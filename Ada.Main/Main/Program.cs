namespace Ada.Main
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using Action64.IngestActions;
    using ActionBase;
    using Actions;
    using CommandLine;
    using log4net;
    using UI.Winforms;

    #endregion

    public static class Program
    {
        #region Static

        // private static readonly ILog infoHeadlessLog = LogManager.GetLogger("InfoHeadlessLogger");
        public const int SuccessCode = 0;

        public const int ErrorCode = 1;

        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region

        /// The main entry point for the application.
        [STAThread]
        public static int Main(string[] args)
        {
// if (Debugger.IsAttached == false) Debugger.Launch();
            int res;

            // alternative to using <startup useLegacyV2RuntimeActivationPolicy="true">, for when started by unit testing
            var isV2Legacy = RuntimePolicyHelper.LegacyV2RuntimeEnabledSuccessfully;

            // sanity check against project System.Data.SQLite.ExtraDlls
            if (!new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite3.dll")).Exists)
                throw new InvalidOperationException("missing a file, (sqlite3.dll)");

            // sanity check against nuget project LeadToolsHelper
            if (!new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Leadtools.Codecs.Cmp.dll")).Exists)
                throw new InvalidOperationException("missing a file, (Leadtools.Codecs.Cmp.dll)");

            AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>();
            AdaActionContainer.Instance.Load<AdaSingleQueryAction>();


            var wOptions = new WindowOptions();

            if (Parser.Default.ParseArguments(args, wOptions))
            {
                if (wOptions.AvIdPath == null && args.Length == 1)
                    wOptions.AvIdPath = args[0];
                RunWithGui(wOptions.AvIdPath);
                res = SuccessCode;
            }
            else
            {
                res = Headless.HeadlessMain(args);
                Console.WriteLine();
            }

            return res;
        }


        private static void RunWithGui(string avIdPath)
        {
            InitializationMethods.PrepareLogging();

            // set initial database directory
            InitializationMethods.SetDbCreationDirectory();

            // Todo: Appropriate?
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("da-dk");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

// Thread.CurrentThread.CurrentCulture.;
            Log.Debug("Start app mode: WinForm");

            GuiRunner.Run(avIdPath);

            Log.Debug("Ada started in Form mode!");
        }

        #endregion
    }
}