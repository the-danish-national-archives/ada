// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The string extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.ExtensionMethods
{
    #region Namespace Using

    using System.IO;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        #region Static

        private static readonly Regex RxNonDigits = new Regex(@"[^\d]+");
        private static readonly Regex RxTrailingDigits = new Regex(@"(\d+)$");

        #endregion

        #region

        public static string AsQuoted(this string s, char quote = '"')
        {
            return quote + s + quote;
        }

        /// <summary>
        ///     The get root folder.
        /// </summary>
        /// <param name="path">
        ///     The path.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetRootFolder(this string path)
        {
            while (true)
            {
                var temp = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(temp)) break;

                path = temp;
            }

            return path;
        }

        /// <summary>
        ///     The get unquoted string.
        /// </summary>
        /// <param name="s">
        ///     The s.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetUnquotedString(this string s)
        {
            s = s.TrimStart('"');
            s = s.TrimEnd('"');
            return s;
        }

        public static string OnlyDigits(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : RxNonDigits.Replace(s, "");
        }

        public static string OnlyTrailingDigits(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : RxTrailingDigits.Match(s).Groups[1].ToString();
        }

        #endregion
    }
}