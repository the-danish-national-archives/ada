namespace Ra.DomainEntities
{
    #region Namespace Using

    using System;
    using System.IO;

    #endregion

    [Serializable]
    public class AViD
    {
        #region Static

        private static readonly string _prefix = "AVID";

        #endregion

        #region  Fields

        private string _archiveCode;


        private string _avSerial;

        #endregion

        #region  Constructors

        public AViD(string fullAViD)
        {
            _archiveCode = ExtractArchiveCode(fullAViD);
            _avSerial = ExtractAVSerial(fullAViD);
        }

        public AViD(string archiveCode, string avSerial)
        {
            _archiveCode = archiveCode;
            _avSerial = avSerial;
        }

        #endregion

        #region Properties

        public string ArchiveCode
        {
            get => _archiveCode;
            private set => _archiveCode = value;
        }

        public string AVSerial
        {
            get => _avSerial;
            private set => _avSerial = value;
        }


        public string FullID => _prefix + "." + _archiveCode + "." + _avSerial;

        public string FullIDNoPrefix => _archiveCode + "." + _avSerial;

        #endregion

        #region

        public static string ExtractArchiveCode(string fullAViD)
        {
            return ExtractAt(fullAViD, 1);
        }

        private static string ExtractAt(string fullAViD, int index)
        {
            var folderSplit = fullAViD.LastIndexOfAny(new[]
            {
                Path.VolumeSeparatorChar,
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            });

            if (folderSplit >= 0) fullAViD = fullAViD.Substring(folderSplit + 1);

            var Split = fullAViD.Split('.');

            if (Split[0].Length < 4) return null;
            var prefix = Split[0].Substring(Split[0].Length - 4);

            if (!string.Equals(prefix, _prefix, StringComparison.CurrentCultureIgnoreCase)) return null;

            if (Split.Length - 1 < index) return null;

            if (Split.Length > 4) return null;

            return Split[index].ToUpper();
        }

        public static string ExtractAVSerial(string fullAViD)
        {
            return ExtractAt(fullAViD, 2);
        }

        public static string ExtractMediaNumber(string fullAViD)
        {
            var mediaNumber = ExtractAt(fullAViD, 3);
            int inttype;
            if (!int.TryParse(mediaNumber, out inttype)) mediaNumber = null;
            return mediaNumber;
        }

        #endregion
    }
}