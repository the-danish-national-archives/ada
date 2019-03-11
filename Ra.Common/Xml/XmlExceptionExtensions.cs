// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlExceptionExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The xml exception extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System.Reflection;
    using System.Xml.Schema;

    #endregion

    /// <summary>
    ///     The xml exception extensions.
    /// </summary>
    public static class XmlExceptionExtensions
    {
        #region

        /// <summary>
        ///     The get error code.
        /// </summary>
        /// <param name="ex">
        ///     The ex.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetErrorCode(this XmlSchemaException ex)
        {
            var value = string.Empty;
            if (ex != null)
            {
                var fi = typeof(XmlSchemaException).GetField(
                    "res",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null) value = fi.GetValue(ex).ToString();
            }

            return value;
        }

        #endregion
    }
}