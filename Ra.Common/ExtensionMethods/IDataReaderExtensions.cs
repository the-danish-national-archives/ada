namespace Ra.Common.ExtensionMethods
{
    #region Namespace Using

    using System.Data;

    #endregion

    public static class IDataReaderExtensions
    {
        #region

        public static string[] ToStringArray(this IDataReader reader)
        {
            var values = new string[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++) values[i] = reader[i].ToString();

            return values;
        }

        #endregion
    }
}