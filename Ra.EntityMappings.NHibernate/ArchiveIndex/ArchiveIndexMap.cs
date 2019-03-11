namespace Ra.EntityMappings.NHibernate.ArchiveIndex
{
    #region Namespace Using

    using DomainEntities.ArchiveIndex;
    using FluentNHibernate.Mapping;

    #endregion

    internal class ArchiveIndexMap : ClassMap<ArchiveIndex>
    {
        #region  Constructors

        public ArchiveIndexMap()
        {
            Map(c => c.SessionKey).Column("sessionKey");
            Id(c => c.Key).Column("archiveIndexKey").GeneratedBy.Native();
            Map(c => c.ArchiveInformationPackageId).Column("archiveInformationPackageID");
            Map(c => c.ArchiveInformationPackageIdPrevious).Column("archiveInformationPackageIDPrevious");
            Map(c => c.ArchivePeriodStart).Column("archivePeriodStart");
            Map(c => c.ArchivePeriodEnd).Column("archivePeriodEnd");
            Map(c => c.ArchiveInformationPacketType).Column("archiveInformationPacketType");
            HasMany(c => c.ArchiveCreators).Inverse().Cascade.AllDeleteOrphan();
            Map(c => c.ArchiveType).Column("archiveType");
            Map(c => c.SystemName).Column("SystemName");
            HasMany(c => c.AlternativeName).KeyColumn("archiveIndexKey").Table("alternativeNames").Element("alternativeName").Cascade.All();
            Map(c => c.SystemPurpose).Column("systemPurpose");
            Map(c => c.SystemContent).Column("systemContent");
            Map(c => c.RegionNum).Column("regionNum");
            Map(c => c.KomNum).Column("komNum");
            Map(c => c.CprNum).Column("cprNum");
            Map(c => c.CvrNum).Column("cvrNum");
            Map(c => c.MatrikNum).Column("matrikNum");
            Map(c => c.BbrNum).Column("bbrNum");
            Map(c => c.WhoSygKod).Column("whoSygKod");
            HasMany(c => c.SourceName).KeyColumn("archiveIndexKey").Table("sourceNames").Element("sourceName").Cascade.All();
            HasMany(x => x.UserName).KeyColumn("archiveIndexKey").Table("userNames").Element("userName").Cascade.All();
            HasMany(x => x.PredecessorName).KeyColumn("archiveIndexKey").Table("predecessorNames").Element("predecessorName").Cascade.All();
            HasOne(c => c.Form).PropertyRef(r => r.ArchiveIndex).Cascade.All().Class<Form>();
            Map(c => c.ContainsDigitalDocuments).Column("containsDigitalDocuments");
            Map(c => c.SearchRelatedOtherRecords).Column("searchRelatedOtherRecords");
            HasMany(c => c.RelatedRecordsName).KeyColumn("archiveIndexKey").Table("relatedRecordsNames").Element("relatedRecordsName").Cascade.All();
            Map(c => c.SystemFileConcept).Column("systemFileConcept");
            Map(c => c.MultipleDataCollection).Column("multipleDataCollection");
            Map(c => c.PersonalDataRestrictedInfo).Column("personalDataRestrictedInfo");
            Map(c => c.OtherAccessTypeRestrictions).Column("otherAccessTypeRestrictions");
            Map(c => c.ArchiveApproval).Column("archiveApproval");
            Map(c => c.ArchiveRestrictions).Column("archiveRestrictions");
            Table("archiveIndex");
        }

        #endregion
    }
}