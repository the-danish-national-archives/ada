namespace Ada.UI.Wpf.ExtensionMethods
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public static class ListKeyValuePairExtensions
    {
        #region

        public static void Add<T, T2>(this IList<KeyValuePair<T, T2>> list, T v1, T2 v2)
        {
            list.Add(new KeyValuePair<T, T2>(v1, v2));
        }

        #endregion
    }
}