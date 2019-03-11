namespace Ada.EntityLoaders
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using Ra.Common;
    using Ra.Common.Xml;
    using Ra.DomainEntities.ArchiveIndex;
    using Ra.XmlEntities.ArchiveIndex;
    using Form = Ra.DomainEntities.ArchiveIndex.Form;
    using FormClass = Ra.DomainEntities.ArchiveIndex.FormClass;

    #endregion

    public static class ArchiveIndexLoader
    {
        #region

        public static ArchiveIndex Load(IArchivalXmlReader reader, BufferedProgressStream stream, BufferedProgressStream schema)
        {
            reader.Open(stream, schema);
            var indexFromXml =
                (archiveIndex)
                reader.Deserialize(typeof(archiveIndex));
            reader.Close();

            var index = new ArchiveIndex
            {
                AlternativeName = indexFromXml.AlternativeName,
                ArchiveApproval = indexFromXml.ArchiveApproval,
                ArchiveCreators = new List<ArchiveCreator>(),
                ArchiveInformationPackageId = indexFromXml.ArchiveInformationPackageID,
                ArchiveInformationPackageIdPrevious = indexFromXml.ArchiveInformationPackageIDPrevious,
                ArchiveInformationPacketType = indexFromXml.ArchiveInformationPacketType,
                ArchivePeriodEnd = indexFromXml.ArchivePeriodEnd,
                ArchivePeriodStart = indexFromXml.ArchivePeriodStart,
                ArchiveRestrictions = indexFromXml.ArchiveRestrictions,
                ArchiveType = indexFromXml.ArchiveType,
                BbrNum = indexFromXml.BbrNum,
                ContainsDigitalDocuments = indexFromXml.ContainsDigitalDocuments,
                CprNum = indexFromXml.CprNum,
                CvrNum = indexFromXml.CvrNum,
                KomNum = indexFromXml.KomNum,
                MatrikNum = indexFromXml.MatrikNum,
                MultipleDataCollection = indexFromXml.MultipleDataCollection,
                OtherAccessTypeRestrictions = indexFromXml.OtherAccessTypeRestrictions,
                PersonalDataRestrictedInfo = indexFromXml.PersonalDataRestrictedInfo,
                PredecessorName = indexFromXml.PredecessorName,
                RegionNum = indexFromXml.RegionNum,
                RelatedRecordsName = indexFromXml.RelatedRecordsName,
                SearchRelatedOtherRecords = indexFromXml.SearchRelatedOtherRecords,
                SessionKey = Guid.NewGuid(),
                SourceName = indexFromXml.SourceName,
                SystemContent = indexFromXml.SystemContent,
                SystemFileConcept = indexFromXml.SystemFileConcept,
                SystemName = indexFromXml.SystemName,
                SystemPurpose = indexFromXml.SystemPurpose,
                UserName = indexFromXml.UserName,
                WhoSygKod = indexFromXml.WhoSygKod
            };

            index.Form = new Form
            {
                FormVersion = indexFromXml.Form?.FormVersion,
                ArchiveIndex = index,
                Classes = new List<FormClass>()
            };

            for (var i = 0; i < indexFromXml.Form?.ClassList.Class.Count; i++)
            {
                var formClass = new FormClass
                {
                    Class =
                        indexFromXml.Form?.ClassList?.Class[i
                        ],
                    Text =
                        indexFromXml.Form?.ClassList
                            ?.ClassText[i],
                    Form = index.Form
                };
                index.Form.Classes.Add(formClass);
            }

            for (var i = 0; i < indexFromXml.ArchiveCreators.CreatorName.Count; i++)
            {
                var creator = new ArchiveCreator
                {
                    ArchiveIndex = index,
                    CreatorName = indexFromXml.ArchiveCreators.CreatorName[i],
                    CreationPeriodStart =
                        indexFromXml.ArchiveCreators.CreationPeriodStart[i],
                    CreationPeriodEnd = indexFromXml.ArchiveCreators.CreationPeriodEnd[i]
                };

                index.ArchiveCreators.Add(creator);
            }

            return index;
        }

        #endregion
    }
}