// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="">
//   
// </copyright>
// <summary>
//   The options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ada
{
    #region Namespace Using

    using CommandLine;
    using CommandLine.Text;

    #endregion

    /// <summary>
    ///     The options.
    /// </summary>
    internal class Options
    {
        #region Properties

        [Option('c', "continue", DefaultValue = false, Required = false, HelpText = "Fortsætter afbrudt test")]
        public bool Continue { get; set; }

        /// <summary>
        ///     Gets or sets the input file.
        /// </summary>
        [Option('i', "input", Required = true, HelpText = "Fuld sti til arkiveringsversion der skal testes.")]
        public string InputFile { get; set; }

        /// <summary>
        ///     Gets or sets the log file folder.
        /// </summary>
        [Option('f', "logfilefolder", Required = false, HelpText = "Logfile mappe sti")]
        public string LogFileFolder { get; set; }

        /// <summary>
        ///     Gets or sets other paths to consider as drives.
        /// </summary>
        [Option('r', "roots", Required = false, HelpText = "Other paths to be considered as drives, split by ';'")]
        public string OtherRoots { get; set; }

        /// <summary>
        ///     Gets or sets the output file.
        /// </summary>
        [Option('o', "output", Required = false, HelpText = "Fuld sti til outputdirectory.")]
        public string OutputFile { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether output is verbose.
        /// </summary>
        [Option('q', "quiet", DefaultValue = false, Required = false, HelpText = "Only show erros in the output (Severity == 1), i.e. not hints")]
        public bool Quiet { get; set; }

        /// <summary>
        ///     Gets or sets the settings file.
        /// </summary>
        [Option('s', "settings", Required = true, HelpText = "Sti til fil med indstillinger")]
        public string SettingsFile { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether output is verbose.
        /// </summary>
        [Option('v', "verbose", Required = false, HelpText = "Vis alle fejl og advarsler")]
        public bool Verbose { get; set; }

        #endregion

        #region

        /// <summary>
        ///     The get usage.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        [HelpOption(HelpText = "Viser denne hjælp")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        #endregion
    }
}