namespace Ada.EntityLoaders
{
    #region Namespace Using

    using System.Collections.Generic;
    using Ra.Common;
    using Ra.Common.Xml;
    using Ra.DomainEntities.ContextDocumentationIndex;

    #endregion

    public static class ContextDocumentationIndexLoader
    {
        #region

        public static ContextDocumentationIndex Load
        (
            IArchivalXmlReader reader,
            BufferedProgressStream stream,
            BufferedProgressStream schema)
        {
            reader.Open(stream, schema);
            var indexFromXml =
                (Ra.XmlEntities.ContextDocumentationIndex.ContextDocumentationIndex)
                reader.Deserialize(typeof(Ra.XmlEntities.ContextDocumentationIndex.ContextDocumentationIndex));

            reader.Close();

            var index = new ContextDocumentationIndex();
            index.Documents = new List<ContextDocumentationDocument>();

            foreach (var docFromXml in indexFromXml.Documents)
            {
                var doc = new ContextDocumentationDocument
                {
                    Authors = new List<ContextDocumentationIndexDocumentAuthor>(),
                    DocumentDate = docFromXml.DocumentDate,
                    DocumentDescription = docFromXml.DocumentDescription,
                    DocumentId = docFromXml.DocumentId,
                    DocumentTitle = docFromXml.DocumentTitle
                };

                foreach (var authorFromXml in docFromXml.Authors)
                {
                    var author = new ContextDocumentationIndexDocumentAuthor
                    {
                        AuthorName = authorFromXml.AuthorName,
                        AuthorInstitution = authorFromXml.AuthorInstitution,
                        Document = doc
                    };

                    doc.Authors.Add(author);
                }

                var category = new DocumentCategoryType();


                var archivalPreservationInformation = docFromXml.DocumentCategory.ArchivalPreservationInformation == null
                    ? null
                    : new ArchivalPreservationInformationType
                    {
                        Category = category,
                        ArchivalInformationOther = docFromXml.DocumentCategory.ArchivalPreservationInformation.ArchivalInformationOther,
                        ArchivalMigrationInformation = docFromXml.DocumentCategory.ArchivalPreservationInformation.ArchivalMigrationInformation
                    };

                var informationOther = docFromXml.DocumentCategory.InformationOther == null
                    ? null
                    : new InformationOtherType
                    {
                        Category = category,
                        InformationOther = docFromXml.DocumentCategory.InformationOther.InformationOther
                    };

                var ingestInformation = docFromXml.DocumentCategory.IngestInformation == null
                    ? null
                    : new IngestInformationType
                    {
                        Category = category,
                        ArchivistNotes =
                            docFromXml.DocumentCategory.IngestInformation
                                .ArchivistNotes,
                        ArchivalInformationOther =
                            docFromXml.DocumentCategory.IngestInformation
                                .ArchivalInformationOther,
                        ArchivalTestNotes =
                            docFromXml.DocumentCategory.IngestInformation
                                .ArchivalTestNotes
                    };

                var operationalInformation = docFromXml.DocumentCategory.OperationalInformation == null
                    ? null
                    : new OperationalInformationType
                    {
                        Category = category,
                        OperationalSystemConvertedInformation =
                            docFromXml.DocumentCategory
                                .OperationalInformation
                                .OperationalSystemConvertedInformation,
                        OperationalSystemInformation =
                            docFromXml.DocumentCategory
                                .OperationalInformation
                                .OperationalSystemInformation,
                        OperationalSystemInformationOther =
                            docFromXml.DocumentCategory
                                .OperationalInformation
                                .OperationalSystemInformationOther,
                        OperationalSystemSOA =
                            docFromXml.DocumentCategory
                                .OperationalInformation
                                .OperationalSystemSOA
                    };

                var submissionInformation = docFromXml.DocumentCategory.SubmissionInformation == null
                    ? null
                    : new SubmissionInformationType
                    {
                        Category = category,
                        ArchivalInformationOther =
                            docFromXml.DocumentCategory
                                .SubmissionInformation
                                .ArchivalInformationOther,
                        ArchivalProvisions =
                            docFromXml.DocumentCategory
                                .SubmissionInformation
                                .ArchivalProvisions,
                        ArchivalTransformationInformation =
                            docFromXml.DocumentCategory
                                .SubmissionInformation
                                .ArchivalTransformationInformation
                    };
                var systemInformation = docFromXml.DocumentCategory.SystemInformation == null
                    ? null
                    : new SystemInformationType
                    {
                        Category = category,
                        SystemAdministrativeFunctions =
                            docFromXml.DocumentCategory
                                .SystemInformation
                                .SystemAdministrativeFunctions,
                        SystemAgencyQualityControl =
                            docFromXml.DocumentCategory
                                .SystemInformation
                                .SystemAgencyQualityControl,
                        SystemContent =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemContent,
                        SystemDataProvision =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemDataProvision,
                        SystemDataTransfer =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemDataTransfer,
                        SystemInformationOther =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemInformationOther,
                        SystemPresentationStructure =
                            docFromXml.DocumentCategory
                                .SystemInformation
                                .SystemPresentationStructure,
                        SystemPreviousSubsequentFunctions =
                            docFromXml.DocumentCategory
                                .SystemInformation
                                .SystemPreviousSubsequentFunctions,
                        SystemPublication =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemPublication,
                        SystemRegulations =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemRegulations,
                        SystemPurpose =
                            docFromXml.DocumentCategory
                                .SystemInformation.SystemPurpose
                    };

                category.ArchivalPreservationInformation = archivalPreservationInformation;
                category.Document = doc;
                category.InformationOther = informationOther;
                category.IngestInformation = ingestInformation;
                category.OperationalInformation = operationalInformation;
                category.SubmissionInformation = submissionInformation;
                category.SystemInformation = systemInformation;

                doc.DocumentCategory = category;
                index.Documents.Add(doc);
            }

            return index;
        }

        #endregion
    }
}