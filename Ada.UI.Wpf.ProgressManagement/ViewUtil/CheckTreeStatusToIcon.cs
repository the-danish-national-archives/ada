namespace Ada.UI.Wpf.ProgressManagement.ViewUtil
{
    #region Namespace Using

    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using Properties;
    using ViewModel;

    #endregion

    public class CheckTreeStatusToIcon : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as CheckTreeItemStatus?;

            Bitmap res;

            switch (status)
            {
                case CheckTreeItemStatus.ToBeDone:
                    res = Icons.led_gray;
                    break;
                case CheckTreeItemStatus.Finished:
                    res = Icons.led_green;
                    break;
                case CheckTreeItemStatus.Error:
                    res = Icons.led_red;
                    break;
                case CheckTreeItemStatus.Warning:
                    res = Icons.led_yellow;
                    break;
                case CheckTreeItemStatus.Running:
                    res = Icons.led_purple;
                    break;
                case CheckTreeItemStatus.Info:
                    res = Icons.led_blue;
                    break;
                case CheckTreeItemStatus.Skipped:
                    res = Icons.led_aqua;
                    break;
                case CheckTreeItemStatus.Unknown:
                case null:
                default:
                    res = Icons.led_orange;
                    break;
            }

            return ToBitmapImage(res);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

        #region

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        #endregion
    }
}