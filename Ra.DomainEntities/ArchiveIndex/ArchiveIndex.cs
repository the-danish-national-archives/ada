namespace Ra.DomainEntities.ArchiveIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class ArchiveIndex : EntityBase
    {
        #region Properties

        public virtual IList<string> AlternativeName { get; set; }

        public virtual string ArchiveApproval { get; set; }

        public virtual IList<ArchiveCreator> ArchiveCreators { get; set; }

        public virtual string ArchiveInformationPackageId { get; set; }

        public virtual string ArchiveInformationPackageIdPrevious { get; set; }

        public virtual bool ArchiveInformationPacketType { get; set; }

        public virtual string ArchivePeriodEnd { get; set; }

        public virtual string ArchivePeriodStart { get; set; }

        public virtual string ArchiveRestrictions { get; set; }

        public virtual bool ArchiveType { get; set; }

        public virtual bool BbrNum { get; set; }

        public virtual bool ContainsDigitalDocuments { get; set; }

        public virtual bool CprNum { get; set; }

        public virtual bool CvrNum { get; set; }

        public virtual Form Form { get; set; }

        public virtual bool KomNum { get; set; }

        public virtual bool MatrikNum { get; set; }

        public virtual bool MultipleDataCollection { get; set; }

        public virtual bool OtherAccessTypeRestrictions { get; set; }

        public virtual bool PersonalDataRestrictedInfo { get; set; }

        public virtual IList<string> PredecessorName { get; set; }

        public virtual bool RegionNum { get; set; }

        public virtual IList<string> RelatedRecordsName { get; set; }

        public virtual bool SearchRelatedOtherRecords { get; set; }
        public virtual Guid SessionKey { get; set; }

        public virtual IList<string> SourceName { get; set; }

        public virtual string SystemContent { get; set; }

        public virtual bool SystemFileConcept { get; set; }

        public virtual string SystemName { get; set; }

        public virtual string SystemPurpose { get; set; }

        public virtual IList<string> UserName { get; set; }

        public virtual bool WhoSygKod { get; set; }

        #endregion
    }
}