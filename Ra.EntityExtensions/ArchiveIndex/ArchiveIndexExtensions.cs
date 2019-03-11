namespace Ra.EntityExtensions.ArchiveIndex
{
    #region Namespace Using

    using System;
    using DomainEntities.ArchiveIndex;

    #endregion

    public static class ArchiveIndexExtensions
    {
        #region

        public static DateTime ArchivePeriodEndAsDateTime(this ArchiveIndex me)
        {
            return XsdDatoTypeParse(me.ArchivePeriodEnd);
        }

        public static DateTime ArchivePeriodStartAsDateTime(this ArchiveIndex me)
        {
            return XsdDatoTypeParse(me.ArchivePeriodStart);
        }

        public static DateTime XsdDatoTypeParse(string date)
        {
            var year = 1700;
            var month = 1;
            var day = 1;

            var dateSplit = date.Split('-');

            if (dateSplit.Length > 2)
                int.TryParse(dateSplit[2], out day);

            if (dateSplit.Length > 1)
                int.TryParse(dateSplit[1], out month);

            if (dateSplit.Length > 0)
                int.TryParse(dateSplit[0], out year);

            return new DateTime(year, month, day);
        }

        #endregion
    }
}