namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    #endregion

    public class TakeScreenShotCommand : DependencyObject, ICommand
    {
        #region Static

        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.Register("Control", typeof(object),
                typeof(TakeScreenShotCommand), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty AutoOpenProperty = DependencyProperty.Register(
            "AutoOpen", typeof(bool), typeof(TakeScreenShotCommand), new PropertyMetadata(default(bool)));

        #endregion

        #region Properties

        public bool AutoOpen
        {
            get => (bool) GetValue(AutoOpenProperty);
            set => SetValue(AutoOpenProperty, value);
        }

        public object Control
        {
            get => GetValue(ControlProperty);
            set => SetValue(ControlProperty, value);
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var ct = Control as FrameworkElement;
            if (ct == null)
                return;

            var scale = 8d;
            var filename = Path.GetTempFileName().Replace(".tmp", ".png");
            var fileInfo = new FileInfo(filename); //dir.FullName + $@"\test{scale}.png");

            if (fileInfo.Exists)
                fileInfo.Delete();

            var imageArray = GetPngImage(ct, scale);

            fileInfo.Refresh();
            using (var fileStream = fileInfo.Create())
            {
                fileStream.Write(imageArray, 0, imageArray.Length);
            }


            if (AutoOpen)
                Process.Start(filename);
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        #region

        /// Gets a Png "screenshot" of the current UIElement
        /// 
        /// UIElement to screenshot
        /// Scale to render the screenshot
        /// Byte array of Png data
        public static byte[] GetPngImage(FrameworkElement source, double scale)
        {
            var actualWidth = source.RenderSize.Width;
            var actualHeight = source.RenderSize.Height;

            var preSize = source.RenderSize;
            var preTrans = source.RenderTransform;
            var preTransL = source.LayoutTransform;
            var preRenderTransformOrigin = source.RenderTransformOrigin;

            var renderWidth = actualWidth * scale;
            var renderHeight = actualHeight * scale;

            if (new[] {renderWidth, renderHeight}.Any(d => d > short.MaxValue))
                return new byte[0];

            var sourceBrush = new VisualBrush(source)
            {
                Stretch = Stretch.Fill,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };


            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                var contentScale = 1 / scale;
                var endPoint = new Point(renderWidth, renderHeight);

                drawingContext.PushTransform(new ScaleTransform(contentScale, contentScale));
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), endPoint));
            }

            var dpi = 96d * scale;

            var renderTarget = new RenderTargetBitmap((int) renderWidth, (int) renderHeight, dpi, dpi, PixelFormats.Default);

            renderTarget.Render(drawingVisual);


            source.RenderTransform = preTrans;
            source.LayoutTransform = preTransL;
            source.RenderTransformOrigin = preRenderTransformOrigin;

            source.Measure(preSize);
            source.Arrange(new Rect(new Point(0d, 0d), preSize));
            source.InvalidateArrange();


            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            //            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //            encoder.QualityLevel = quality;
            //            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            byte[] _imageArray;

            using (var outputStream = new MemoryStream())
            {
                encoder.Save(outputStream);
                _imageArray = outputStream.ToArray();
            }


            return _imageArray;
        }

        #endregion
    }
}