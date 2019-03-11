namespace Ra.EntityMappings.NHibernate.FileIndex
{
    #region Namespace Using

    using System.Diagnostics.CodeAnalysis;
    using DomainEntities.Documents;
    using FluentNHibernate.Mapping;

    #endregion

    /// <summary>
    ///     Fluent NHibernate mapping for the FileIndexEntry Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class DocIndexEntryMap : ClassMap<DocIndexEntry>
    {
        #region  Constructors

        public DocIndexEntryMap()
        {
            Id(c => c.Key).Column("docKey").GeneratedBy.Native();
            Map(c => c.DocumentId).Column("dID");
            Map(c => c.MediaId).Column("medieNumber");
            Map(c => c.DocumentFolder).Column("relativePath");
            Map(c => c.ParentId).Column("pID");
            Map(c => c.OriginalFileName).Column("originalFileName");
            Map(c => c.Extension).Column("originalExtension");
            Map(c => c.SubmissionFileType).Column("archiveExtension");
            Map(c => c.GmlXsd).Column("gmlXSD");

            Table("documents");
        }

        #endregion
    }
}