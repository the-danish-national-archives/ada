namespace Ra.DomainEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The context documentation document.
    /// </summary>
    public class ContextDocumentationDocument : EntityBase
    {
        #region  Fields

        private string materializedPath;

        #endregion

        #region Properties

        public virtual string archiveExtension { get; set; } = ""; // = ".tif";

        /// <summary>
        ///     Gets or sets the authors.
        /// </summary>
        public virtual IList<ContextDocumentationIndexDocumentAuthor> Authors { get; set; }

        /// <summary>
        ///     Gets or sets the document category.
        /// </summary>
        public virtual DocumentCategoryType DocumentCategory { get; set; }

        /// <summary>
        ///     Gets or sets the document date.
        /// </summary>
        public virtual string DocumentDate { get; set; }

        /// <summary>
        ///     Gets or sets the document description.
        /// </summary>
        public virtual string DocumentDescription { get; set; }

        /// <summary>
        ///     Gets or sets the document id.
        /// </summary>
        public virtual string DocumentId { get; set; }

        /// <summary>
        ///     Gets or sets the document title.
        /// </summary>
        public virtual string DocumentTitle { get; set; }

        public virtual string MaterializedPath
        {
            get => @"\ContextDocumentation\docCollection1\" + DocumentId;
            set => materializedPath = value;
        }


        public virtual string MediaId { get; set; } = "1";

        public virtual string RelativePath { get; set; } = ""; // = "docCollection1";
        public virtual Guid SessionKey { get; set; } = new Guid();

        #endregion
    }
}