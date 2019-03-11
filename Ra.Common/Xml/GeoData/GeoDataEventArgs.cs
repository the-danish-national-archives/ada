namespace Ra.Common.Xml.GeoData
{
    #region Namespace Using

    using System;
    using System.IO;

    #endregion

    public class GeoDataEventArgs : EventArgs
    {
        #region  Constructors

        public GeoDataEventArgs(GeoDataEventType eventType, FileInfo sourceFileInfo)
        {
            EventType = eventType;
            SourceFileInfo = sourceFileInfo;
        }

        #endregion

        #region Properties

        public GeoDataEventType EventType { get; }
        public FileInfo SourceFileInfo { get; }

        #endregion
    }
}