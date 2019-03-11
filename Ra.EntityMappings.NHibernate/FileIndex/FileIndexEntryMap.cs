namespace Ra.EntityMappings.NHibernate.FileIndex
{
    #region Namespace Using

    using System.Diagnostics.CodeAnalysis;
    using DomainEntities.FileIndex;
    using FluentNHibernate.Mapping;

    #endregion

    /// <summary>
    ///     Fluent NHibernate mapping for the FileIndexEntry Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class FileIndexEntryMap : ClassMap<FileIndexEntry>
    {
        #region  Constructors

        public FileIndexEntryMap()
        {
            Id(c => c.Key).Column("fileKey").GeneratedBy.Native();
            Map(c => c.MediaNumber).Column("mediaNumber");
            Map(c => c.RelativePath).Column("relativePath");
            Map(c => c.FileName).Column("fileName");
            Map(c => c.Extension).Column("extension");
            Map(c => c.Md5).Column("md5");
            Map(c => c.foN).Column("foN");

            Table("files");
        }

        #endregion
    }
}