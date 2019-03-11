namespace Ra.Common.Xml.GeoData
{
    public class Bounds
    {
        #region  Constructors

        public Bounds(Point lowerCorner, Point upperCorner)
        {
            LowerCorner = lowerCorner;
            UpperCorner = upperCorner;
        }

        public Bounds(double lowerCornerX, double lowerCornerY, double upperCornerX, double upperCornerY)
        {
            LowerCorner = new Point(lowerCornerX, lowerCornerY);
            UpperCorner = new Point(upperCornerX, upperCornerY);
        }

        public Bounds(string lowerCornerXmlValue, string upperCornerXmlValue)
        {
            LowerCorner = new Point(lowerCornerXmlValue);
            UpperCorner = new Point(upperCornerXmlValue);
        }

        #endregion

        #region Properties

        public Point LowerCorner { get; set; }

        public Point UpperCorner { get; set; }

        #endregion

        #region

        public bool IsValidArea()
        {
            return LowerCorner.X < UpperCorner.X && LowerCorner.Y < UpperCorner.Y;
        }

        public bool IsWithinOrEqualTo(Bounds bounds)
        {
            return LowerCorner.X >= bounds.LowerCorner.X && LowerCorner.Y >= bounds.LowerCorner.Y
                                                         && UpperCorner.X <= bounds.UpperCorner.X && UpperCorner.Y <= bounds.UpperCorner.Y;
        }

        #endregion
    }
}