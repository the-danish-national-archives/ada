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
    ///     Options for starting in windowed mode.
    /// </summary>
    internal class WindowOptions
    {
        #region Properties

        /// <summary>
        ///     Path to be opened in Windowed mode
        /// </summary>
        [Option('w', null, Required = false, HelpText = "Path to be opened in Windowed mode")]
        public string AvIdPath { get; set; }

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