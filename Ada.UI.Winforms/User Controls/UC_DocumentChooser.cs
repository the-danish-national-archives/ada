namespace Ada.UI.Winforms.User_Controls
{
    #region Namespace Using

    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    #endregion

    public partial class UC_DocumentChooser : Form
    {
        #region  Fields

        private readonly UC_DocumentViewer documentViewer1;

        #endregion

        #region  Constructors

        public UC_DocumentChooser(UC_DocumentViewer uC_DocumentViewer1)
        {
            InitializeComponent();
            documentViewer1 = uC_DocumentViewer1;
        }

        #endregion

        #region

        private void button1_Click(object sender, EventArgs e)
        {
            OpenDocumentFileForId();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void OpenDocumentFileForId()
        {
            var id = textBox1.Text;
            int i;
            if (!int.TryParse(id, out i)) Dispose();

            var x = new UC_DocumentViewer();
            //x.ShowDocumentFile(i.ToString());
            new Task(() => { documentViewer1.ShowDocumentFileForId(i); }).Start();
            //this.Dispose();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Determine whether the keystroke is enter.
            if (e.KeyCode == Keys.Return) OpenDocumentFileForId();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion
    }
}