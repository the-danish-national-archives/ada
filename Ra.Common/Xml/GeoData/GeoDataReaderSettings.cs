namespace Ra.Common.Xml.GeoData
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public class GeoDataReaderSettings
    {
        #region  Constructors

        public GeoDataReaderSettings
        (
            List<string> acceptableDatums = null,
            List<string> acceptableDimensions = null,
            Bounds bounds = null,
            bool? enforceBounds = null,
            bool? checkFeatures = null)
        {
            AcceptableDatums = acceptableDatums ?? new List<string> {"EPSG:25831", "EPSG:25832", "EPSG:25833"};
            AcceptableDimensions = acceptableDimensions ?? new List<string> {"2", "3"};
            Bounds = bounds ?? new Bounds(212481.60, 6019669.40, 961440.75, 6510422.51);
            EnforceBounds = enforceBounds ?? true;
            CheckFeatures = checkFeatures ?? true;
        }

        #endregion

        #region Properties

        public List<string> AcceptableDatums { get; set; }
        public List<string> AcceptableDimensions { get; set; }
        public Bounds Bounds { get; set; }
        public bool CheckFeatures { get; set; }
        public bool EnforceBounds { get; set; }

        #endregion
    }
}