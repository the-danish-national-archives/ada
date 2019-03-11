namespace Ra.DocumentInvestigator.UsercontrolsWrappers
{
    #region Namespace Using

    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Leadtools;
    using Leadtools.Codecs;
    using Leadtools.WinForms;
    using OldForWinforms;

    #endregion

    public partial class RasterPictureBoxWrapper : UserControl
    {
        #region  Fields

        //Codex og variabler
        private readonly RasterCodecs codecs;

        internal RasterPictureBox rasterPictureBox1;

        #endregion

        #region  Constructors

        public RasterPictureBoxWrapper()
        {
            //Skal ske før InitializeComponent !!!
            //RasterSupport.Unlock(RasterSupportType.PdfRead, ""); //Kræver plugin eller kode
            //Leadtools.Codecs.RasterCodecs.Startup();
            codecs = new RasterCodecs();

            InitializeComponent();

            SuspendLayout();

            rasterPictureBox1 = new RasterPictureBox();

            // 
            // rasterPictureBox1
            // 
            rasterPictureBox1.BorderStyle = BorderStyle.Fixed3D;
            rasterPictureBox1.Dock = DockStyle.Fill;
            rasterPictureBox1.Image = null;
            rasterPictureBox1.Location = new Point(0, 0);
            rasterPictureBox1.Name = "rasterPictureBox1";
            rasterPictureBox1.SizeMode = RasterPictureBoxSizeMode.Fit;
            rasterPictureBox1.TabIndex = 9;
            rasterPictureBox1.TabStop = false;
            rasterPictureBox1.MouseUp += (sender, args) => OnMouseUp(args);

            Controls.Add(rasterPictureBox1);

            ResumeLayout(false);
        }

        #endregion

        #region Properties

        public bool HasImage => rasterPictureBox1.Image != null;

        public string SizeMode
        {
            get => rasterPictureBox1.SizeMode.ToString();
            set
            {
                var temp = rasterPictureBox1.SizeMode;
                Enum.TryParse(value, out temp);
                rasterPictureBox1.SizeMode = temp;
            }
        }

        #endregion

        #region

        public void LoadPage(DocPage docPage)
        {
            rasterPictureBox1.Image = codecs.Load(
                docPage.FileName,
                0,
                CodecsLoadByteOrder.BgrOrGray,
                docPage.PageNo,
                docPage.PageNo);
        }

        public void TryLoadPage(DocPage docPage)
        {
            try
            {
                rasterPictureBox1.Image = codecs.Load(
                    docPage.FileName,
                    0,
                    CodecsLoadByteOrder.BgrOrGray,
                    docPage.PageNo,
                    docPage.PageNo);
            }
            catch (RasterException e)
            {
                //MessageBox.Show(e.Message);
                if (e.Code == RasterExceptionCode.ExtGrayNotEnabled)
                {
                    MessageBox.Show(
                        "TIFF 12 bit (Extended Graytone) er ikke understøttet.\nKonverter til alm. 1, 8 eller 24 bit TIFF");
                    rasterPictureBox1.Image = codecs.Load(
                        docPage.FileName,
                        24,
                        CodecsLoadByteOrder.BgrOrGray,
                        docPage.PageNo,
                        docPage.PageNo);
                }
                else
                {
                    MessageBox.Show("Fejl ved visning af billede!\nFejlbeskrivelse: " + e.Message);
                    // rasterPictureBox1.Image = codecs.Load(docPageList[listBox1.SelectedIndex].FileName, 8, Leadtools.Codecs.CodecsLoadByteOrder.Rgb, docPageList[listBox1.SelectedIndex].PageNo, docPageList[listBox1.SelectedIndex].PageNo);                        
                }
            }
        }

        #endregion
    }
}