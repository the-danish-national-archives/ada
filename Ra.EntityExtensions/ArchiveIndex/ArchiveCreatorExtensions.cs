namespace Ra.EntityExtensions.ArchiveIndex
{
    #region Namespace Using

    using System;
    using DomainEntities.ArchiveIndex;

    #endregion

    public static class ArchiveCreatorExtensions
    {
        #region

        public static DateTime CreationPeriodEndAsDateTime(this ArchiveCreator me)
        {
            return ArchiveIndexExtensions.XsdDatoTypeParse(me.CreationPeriodEnd);
        }

        public static DateTime CreationPeriodStartAsDateTime(this ArchiveCreator me)
        {
            return ArchiveIndexExtensions.XsdDatoTypeParse(me.CreationPeriodStart);
        }

        #endregion
    }
}