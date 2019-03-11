namespace Ra.DomainEntities
{
    #region Namespace Using

    using System;

    #endregion

    public enum FileTypeEnum
    {
        Xml,

        Tif,

        Mp3,

        Mpg,

        Jp2,

        Gml,

        Xsd,

        Wav,

        All
    }

    public static class FileTypeEnumExtensions
    {
        #region

        public static string GetExtension(this FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.Xml:
                    return "xml";
                case FileTypeEnum.Tif:
                    return "tif";
                case FileTypeEnum.Mp3:
                    return "mp3";
                case FileTypeEnum.Wav:
                    return "wav";
                case FileTypeEnum.Mpg:
                    return "mpg";
                case FileTypeEnum.Jp2:
                    return "jp2";
                case FileTypeEnum.Gml:
                    return "gml";
                case FileTypeEnum.Xsd:
                    return "xsd";
                case FileTypeEnum.All:
                    return "";

                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }

        #endregion
    }
}