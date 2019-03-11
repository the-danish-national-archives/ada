namespace Ada.UI.Winforms.User_Controls
{
    #region Namespace Using

    using System;
    using System.Drawing;
    using System.Windows.Forms;

    #endregion

    public partial class UC_DocumentViewerForm : Form
    {
        #region  Fields

        private Point globalMousePos;

        //Benyttes til at fastholde oprindelige scroll positioner i forb. med scroll vha mus
        private Point globalScrollPos;

        #endregion

        #region  Constructors

        public UC_DocumentViewerForm()
        {
            //Leadtools.Codecs.RasterCodecs.Startup();
            InitializeComponent();
            TopMost = false;
        }

        #endregion

        #region

        private void button1_Click(object sender, EventArgs e)
        {
            rasterImageViewer1.SizeMode = "FitWidth";
            //Opdatering med H/B og Zoom ratio
            UpdateFormCaption();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            rasterImageViewer1.SizeMode = "Normal";
            //Opdatering med H/B og Zoom ratio
            UpdateFormCaption();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rasterImageViewer1.SizeMode = "Fit";
            //Opdatering med H/B og Zoom ratio
            UpdateFormCaption();
        }

        private void ImgViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }


        private void ImgViewForm_Load(object sender, EventArgs e)
        {
        }

        private void rasterImageViewer1_ImageChanged(object sender, EventArgs e)
        {
            //Opdatering med H/B og Zoom ratio
            UpdateFormCaption();
        }

        private void rasterImageViewer1_MouseDown(object sender, MouseEventArgs e)
        {
            globalScrollPos = rasterImageViewer1.ScrollPosition;
            globalMousePos.X = e.X;
            globalMousePos.Y = e.Y;

            if (e.Button == MouseButtons.Left) rasterImageViewer1.Cursor = Cursors.Hand;
        }


        private void rasterImageViewer1_MouseMove(object sender, MouseEventArgs e)
        {
            //Håndtering af scroll vha mouse move
            if (e.Button == MouseButtons.Left)
                rasterImageViewer1.ScrollPosition = new Point(globalScrollPos.X - (e.X - globalMousePos.X),
                    globalScrollPos.Y - (e.Y - globalMousePos.Y));
        }


        private void rasterImageViewer1_MouseUp(object sender, MouseEventArgs e)
        {
            rasterImageViewer1.Cursor = Cursors.Default;

            if (e.Button == MouseButtons.Right) Hide();
        }

        private void rasterImageViewer1_Resize(object sender, EventArgs e)
        {
        }

        private void rasterImageViewer1_ScaleFactorChanged(object sender, EventArgs e)
        {
        }

        private void UpdateFormCaption()
        {
            var ratio = 0;
            var ratioStr = "";

            if (rasterImageViewer1.CurrentXScaleFactor < 1)
            {
                ratio = (int) (1 / rasterImageViewer1.CurrentXScaleFactor);
                ratioStr = "Zoom 1:" + ratio;
            }
            else
            {
                ratio = (int) (rasterImageViewer1.CurrentXScaleFactor / 1);
                ratioStr = "Zoom " + ratio + ":1";
            }


            Text = rasterImageViewer1.ImageWidth + "x" +
                   rasterImageViewer1.ImageHeight +
                   "  " + ratioStr;
        }

        #endregion
    }
}