namespace Ra.Common.Xml.GeoData
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    #endregion

    public class Point
    {
        #region  Constructors

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(string x, string y)
        {
            SetXandY(x, y);
        }

        public Point(string xmlCoordinateValue)
        {
            try
            {
                var values = xmlCoordinateValue.Split(' ');
                SetXandY(values[0], values[1]);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{nameof(xmlCoordinateValue)} must be of the format 'x y'", e);
            }
        }

        #endregion

        #region Properties

        public double X { get; private set; }
        public double Y { get; private set; }

        #endregion

        #region

        public bool IsWithin(Bounds bounds)
        {
            return X >= bounds.LowerCorner.X && Y >= bounds.LowerCorner.Y
                                             && X <= bounds.UpperCorner.X && Y <= bounds.UpperCorner.Y;
        }

        public static List<Point> PointListFromString(string stringList, GisDimensions dimension)
        {
            var result = new List<Point>();
            try
            {
                var values = stringList.Split(' ');
                var index = 0;
                while (index < values.Length)
                {
                    var point = new Point(values[index], values[index + 1]);
                    index += (int) dimension;
                    result.Add(point);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, e);
            }

            return result;
        }

        private void SetXandY(string x, string y)
        {
            try
            {
                X = Convert.ToDouble(x, NumberFormatInfo.InvariantInfo);
                Y = Convert.ToDouble(y, NumberFormatInfo.InvariantInfo);
            }
            catch (Exception e)
            {
                throw new ArgumentException("x or y can't be converted to Double", e);
            }
        }

        #endregion
    }


    public enum GisDimensions
    {
        Two = 2,
        Three = 3
    }
}