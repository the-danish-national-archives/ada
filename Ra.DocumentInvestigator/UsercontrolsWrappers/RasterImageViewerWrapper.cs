namespace Ra.DocumentInvestigator.UsercontrolsWrappers
{
    #region Namespace Using

    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Leadtools;
    using Leadtools.Codecs;
    using Leadtools.WinForms;

    #endregion

    public partial class RasterImageViewerWrapper : UserControl
    {
        #region  Fields

        //Codex og variabler
        private RasterCodecs codecs;

        internal RasterImageViewer rasterImageViewer1;

        #endregion

        #region  Constructors

        public RasterImageViewerWrapper()
        {
            //Skal ske før InitializeComponent !!!
            //RasterSupport.Unlock(RasterSupportType.PdfRead, ""); //Kræver plugin eller kode
            //Leadtools.Codecs.RasterCodecs.Startup();
            codecs = new RasterCodecs();

            InitializeComponent();

            SuspendLayout();

            rasterImageViewer1 = new RasterImageViewer();

            rasterImageViewer1.Dock = DockStyle.Fill;
            rasterImageViewer1.EnableScrollingInterface = true;
            rasterImageViewer1.HorizontalAlignMode = RasterPaintAlignMode.Center;
            rasterImageViewer1.Location = new Point(0, 0);
            rasterImageViewer1.Name = "rasterImageViewer1";
            //            this.rasterImageViewer1.Size = new System.Drawing.Size(513, 583);
            //            this.rasterImageViewer1.TabIndex = 1;
            rasterImageViewer1.VerticalAlignMode = RasterPaintAlignMode.Center;
            rasterImageViewer1.MouseMove += (sender, args) => OnMouseMove(args);
            rasterImageViewer1.ScaleFactorChanged += (sender, args) => OnScaleFactorChanged(args);
            rasterImageViewer1.MouseDown += (sender, args) => OnMouseMove(args);
            rasterImageViewer1.Resize += (sender, args) => OnResize(args);
            rasterImageViewer1.MouseUp += (sender, args) => OnMouseUp(args);
            rasterImageViewer1.ImageChanged += (sender, args) => OnImageChanged(args);

            Controls.Add(rasterImageViewer1);

            ResumeLayout(false);
        }

        #endregion

        #region Properties

        public double CurrentXScaleFactor
        {
            get => rasterImageViewer1.CurrentXScaleFactor;
            set => rasterImageViewer1.CurrentXScaleFactor = value;
        }

        public bool EnableScrollingInterface
        {
            get => rasterImageViewer1.EnableScrollingInterface;
            set => rasterImageViewer1.EnableScrollingInterface = value;
        }

        public string HorizontalAlignMode
        {
            get => rasterImageViewer1.HorizontalAlignMode.ToString();
            set
            {
                var temp = rasterImageViewer1.HorizontalAlignMode;
                Enum.TryParse(value, out temp);
                rasterImageViewer1.HorizontalAlignMode = temp;
            }
        }

        public int ImageHeight => rasterImageViewer1.Image.Height;

        public int ImageWidth => rasterImageViewer1.Image.Width;

        public Point ScrollPosition
        {
            get => rasterImageViewer1.ScrollPosition;
            set => rasterImageViewer1.ScrollPosition = value;
        }

        public string SizeMode
        {
            get => rasterImageViewer1.SizeMode.ToString();
            set
            {
                var temp = rasterImageViewer1.SizeMode;
                Enum.TryParse(value, out temp);
                rasterImageViewer1.SizeMode = temp;
            }
        }

        public string VerticalAlignMode
        {
            get => rasterImageViewer1.VerticalAlignMode.ToString();
            set
            {
                var temp = rasterImageViewer1.VerticalAlignMode;
                Enum.TryParse(value, out temp);
                rasterImageViewer1.VerticalAlignMode = temp;
            }
        }

        #endregion

        #region

        public event EventHandler ImageChanged;

        public void OnImageChanged(EventArgs e)
        {
            ImageChanged?.Invoke(this, e);
        }

        public void OnScaleFactorChanged(EventArgs e)
        {
            ScaleFactorChanged?.Invoke(this, e);
        }

        public event EventHandler ScaleFactorChanged;

        public void SetImage(RasterPictureBoxWrapper rasterPictureBoxWrapper)
        {
            rasterImageViewer1.Image = rasterPictureBoxWrapper?.rasterPictureBox1?.Image?.Clone(); //!!! Clone
        }

        #endregion
    }
}