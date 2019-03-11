namespace Ra.Test.DocumentInvestigator.UnitTests.Utilities
{
    #region Namespace Using

    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    #endregion

    public static class LogTools
    {
        #region

        public static void ConfigureWithConsole()
        {
            var h = (Hierarchy) LogManager.GetRepository();
            h.Root.Level = Level.All;
            h.Root.AddAppender(CreateConsoleAppender());
            h.Configured = true;
        }

        public static void ConfigureWithFile(string appCode)
        {
            var h = (Hierarchy) LogManager.GetRepository();
            h.Root.Level = Level.All;
            h.Root.AddAppender(CreateFileAppender(appCode));
            h.Configured = true;
        }

        private static IAppender CreateConsoleAppender()
        {
            var appender = new ConsoleAppender {Name = "ConsoleAppender"};
            var layout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger [%property{NDC}] - %message%newline"
            };
            layout.ActivateOptions();
            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }

        private static IAppender CreateFileAppender(string appCode)
        {
            var appender = new RollingFileAppender
            {
                Name = "RollingFileAppender",
                File = string.Format(@"c:\logs\{0}\{0}.log", appCode),
                AppendToFile = false,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "1000MB"
            };
            var layout = new PatternLayout
            {
                ConversionPattern =
                    "% newline % date % -5level % logger – % message – % property % newline"
            };
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }

        #endregion
    }
}