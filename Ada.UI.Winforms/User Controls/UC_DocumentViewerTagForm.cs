namespace Ada.UI.Winforms.User_Controls
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Printing;
    using Ra.DocumentInvestigator.OldForWinforms;

    #endregion

    public partial class UC_DocumentViewerTagForm : Form
    {
        #region Static

        private static readonly TiffLeadToolsProcessor Tiffery = new TiffLeadToolsProcessor();

        #endregion

        #region  Fields

        private List<TifTag> tagList = new List<TifTag>();

        #endregion

        #region  Constructors

        public UC_DocumentViewerTagForm()
        {
            InitializeComponent();
        }

        #endregion

        #region

        private void buttonGemSom_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                MessageBox.Show("Gemt som: " + saveFileDialog1.FileName);
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            var sap = new SA_DocumentPrint();
            sap.PrintTekst(richTextBox1);
        }

        public void ClearTags()
        {
            tagList.Clear();
            richTextBox1.Clear();
        }


        private void DataBut_Click(object sender, EventArgs e)
        {
            if (DataBut.FlatStyle == FlatStyle.Standard)
            {
                DataBut.FlatStyle = FlatStyle.Flat;
                DataBut.BackColor = SystemColors.GradientActiveCaption;

                GroupBut.FlatStyle = FlatStyle.Standard;
                GroupBut.BackColor = SystemColors.Control;

                HintBut.FlatStyle = FlatStyle.Standard;
                HintBut.BackColor = SystemColors.Control;
            }

            ShowTags();
        }

        private void GroupBut_Click(object sender, EventArgs e)
        {
            if (GroupBut.FlatStyle == FlatStyle.Standard)
            {
                GroupBut.FlatStyle = FlatStyle.Flat;
                GroupBut.BackColor = SystemColors.GradientActiveCaption;

                DataBut.FlatStyle = FlatStyle.Standard;
                DataBut.BackColor = SystemColors.Control;

                HintBut.FlatStyle = FlatStyle.Standard;
                HintBut.BackColor = SystemColors.Control;
            }

            ShowTags();
        }

        private void HintBut_Click(object sender, EventArgs e)
        {
            if (HintBut.FlatStyle == FlatStyle.Standard)
            {
                HintBut.FlatStyle = FlatStyle.Flat;
                HintBut.BackColor = SystemColors.GradientActiveCaption;

                DataBut.FlatStyle = FlatStyle.Standard;
                DataBut.BackColor = SystemColors.Control;

                GroupBut.FlatStyle = FlatStyle.Standard;
                GroupBut.BackColor = SystemColors.Control;
            }

            ShowTags();
        }


        public void ReadTags(string fileName, int pageNo)
        {
            tagList = Tiffery.GetDocumentTiffTags(fileName, pageNo);
            ShowTags();
        }


        public void ShowTags()
        {
            //Indlæs og vis Tags
            richTextBox1.Clear();

            //tagList = SA_TifTagInfo.GetDocumentTiffTags(fileName, pageNo);
            //List<SA_TifTagInfo.tifTag> tags = SA_TifTagInfo.GetDocumentTiffTags(fileName, pageNo);
            foreach (var tag in tagList)
            {
                var tagLine = tag.name.PadRight(25, ' ') + " = " + tag.tagNo.ToString().PadRight(5, ' ');

                if (HintBut.FlatStyle == FlatStyle.Flat)
                    tagLine = tagLine + " " + tag.hint;
                else if (DataBut.FlatStyle == FlatStyle.Flat)
                    tagLine = tagLine + " " + tag.data;
                else if (GroupBut.FlatStyle == FlatStyle.Flat) tagLine = tagLine + " " + tag.gruppe;

                richTextBox1.AppendText(tagLine + "\r\n");
            }
        }

        private void TagViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        #endregion
    }
}