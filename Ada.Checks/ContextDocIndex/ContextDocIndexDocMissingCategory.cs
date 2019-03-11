namespace Ada.Checks.ContextDocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.ContextDocumentationIndex;

    #endregion

    public class ContextDocIndexDocMissingCategory : AdaAvViolation
    {
        #region  Constructors

        public ContextDocIndexDocMissingCategory(string docId, string title)
            : base("4.C.4_1")
        {
            DocId = docId;
            Title = title;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string DocId { get; set; }

        [AdaAvCheckNotificationTag]
        public string Title { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ContextDocumentationDocument doc)
        {
            if (!IsAnnotated(doc.DocumentCategory)) yield return new ContextDocIndexDocMissingCategory(doc.DocumentId, doc.DocumentTitle);
        }

        public static bool IsAnnotated(DocumentCategoryType type)
        {
            return type.InformationOther != null && IsAnnotated(type.InformationOther)
                   || type.ArchivalPreservationInformation != null && IsAnnotated(type.ArchivalPreservationInformation)
                   || type.IngestInformation != null && IsAnnotated(type.IngestInformation)
                   || type.OperationalInformation != null && IsAnnotated(type.OperationalInformation)
                   || type.SubmissionInformation != null && IsAnnotated(type.SubmissionInformation)
                   || type.SystemInformation != null && IsAnnotated(type.SystemInformation);
        }

        public static bool IsAnnotated(ArchivalPreservationInformationType type)
        {
            return type.ArchivalInformationOther ||
                   type.ArchivalMigrationInformation;
        }

        public static bool IsAnnotated(InformationOtherType type)
        {
            return type.InformationOther;
        }

        public static bool IsAnnotated(IngestInformationType type)
        {
            return type.ArchivalInformationOther ||
                   type.ArchivalTestNotes ||
                   type.ArchivistNotes;
        }

        public static bool IsAnnotated(OperationalInformationType type)
        {
            return type.OperationalSystemConvertedInformation ||
                   type.OperationalSystemInformation ||
                   type.OperationalSystemInformationOther ||
                   type.OperationalSystemSOA;
        }

        public static bool IsAnnotated(SubmissionInformationType type)
        {
            return type.ArchivalInformationOther ||
                   type.ArchivalProvisions ||
                   type.ArchivalTransformationInformation;
        }

        public static bool IsAnnotated(SystemInformationType type)
        {
            return type.SystemAdministrativeFunctions ||
                   type.SystemAgencyQualityControl ||
                   type.SystemContent ||
                   type.SystemDataProvision ||
                   type.SystemDataTransfer ||
                   type.SystemInformationOther ||
                   type.SystemPresentationStructure ||
                   type.SystemPreviousSubsequentFunctions ||
                   type.SystemPublication ||
                   type.SystemPurpose ||
                   type.SystemRegulations;
        }

        #endregion
    }
}