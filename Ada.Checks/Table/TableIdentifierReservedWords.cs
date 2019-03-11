namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using ChecksBase;

    #endregion

    public class TableIdentifierReservedWords : AdaAvViolation
    {
        #region  Constructors

        public TableIdentifierReservedWords(string entityName, string value)
            : base("6.C_28")
        {
            EntityName = entityName;
            Value = value;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string EntityName { get; set; }

        [AdaAvCheckNotificationTag]
        public string Value { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(string entityName, string value)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ada.Checks.Sql99ReservedWords.txt");
            var reservedWordsList = stream.ReadLines(Encoding.UTF8).ToList();

            if (reservedWordsList.Contains(value?.ToUpper()))
                yield return new TableIdentifierReservedWords(entityName, value);
        }

        #endregion
    }

    public static class StreamExtensions
    {
        #region

        public static IEnumerable<string> ReadLines(this Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null) yield return line;
            }
        }

        #endregion
    }
}