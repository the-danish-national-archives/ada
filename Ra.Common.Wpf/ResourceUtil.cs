namespace Ra.Common.Wpf
{
    #region Namespace Using

    using System.IO;
    using System.Linq;
    using System.Reflection;

    #endregion

    public static class ResourceUtil
    {
        #region

        /// <summary>
        ///     Get the list of all emdedded resources in the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>An array of fully qualified resource names</returns>
        public static string[] GetEmbeddedResourceNames(Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceNames();
        }

        /// <summary>
        ///     Takes the full name of a resource and loads it in to a stream.
        /// </summary>
        /// <param name="resourceName">
        ///     Assuming an embedded resource is a file
        ///     called info.png and is located in a folder called Resources, it
        ///     will be compiled in to the assembly with this fully qualified
        ///     name: Full.Assembly.Name.Resources.info.png. That is the string
        ///     that you should pass to this method.
        /// </param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }

        public static Stream GetEmbeddedResourceStreamFromPath<T>(string resourcePath)
        {
            resourcePath = resourcePath.Replace(@"/", ".");
            resourcePath = resourcePath.Replace(@"\", ".");
            resourcePath = resourcePath.ToLowerInvariant();
            resourcePath = GetEmbeddedResourceNames(typeof(T).Assembly).FirstOrDefault(r => r.ToLowerInvariant().Contains(resourcePath));

            if (resourcePath == null)
                throw new FileNotFoundException("Resource not found");

            return GetEmbeddedResourceStream(resourcePath, typeof(T).Assembly);
        }

        #endregion
    }
}