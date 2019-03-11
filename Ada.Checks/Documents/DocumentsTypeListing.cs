namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities;

    #endregion

    public class DocumentsTypeListing : AdaAvDynamicFromSql
    {
        #region  Constructors

        protected DocumentsTypeListing()
            : base("4.G_15")
        {
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            IEnumerable<(string countName, string ext)> typeToCount =
                    Enum.GetValues(typeof(FileTypeEnum))
                        .OfType<FileTypeEnum>()
                        .Select(e => (e.ToString() + "Count", e.GetExtension()))
                ;


            var joinClauses = typeToCount.Select(
                t => $"(SELECT COUNT(*) AS {t.countName} FROM documents"
                     + (string.IsNullOrWhiteSpace(t.ext)
                         ? ")"
                         : $" WHERE UPPER(archiveExtension)=='{t.ext.ToUpperInvariant()}')"));


            return @"SELECT * FROM " + string.Join(" JOIN ", joinClauses);
        }

        #endregion
    }
}