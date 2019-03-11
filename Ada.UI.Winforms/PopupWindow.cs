namespace Ada.UI.Winforms
{
    #region Namespace Using

    using System.Windows.Forms;

    #endregion

    public class PopupWindow : Form
    {
        #region Static

        public static bool hidden;

        #endregion

        #region  Fields

        private readonly MainForm mainForm;

        #endregion

        #region  Constructors

        public PopupWindow(Form parent)
        {
            mainForm = (MainForm) parent;
            Height = Screen.PrimaryScreen.Bounds.Height * 2 / 3;
            Width = Screen.PrimaryScreen.Bounds.Width * 2 / 3;
            hidden = false;
        }

        #endregion

        #region

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
            hidden = true;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Processes a command key - catch Ctrl + D (Open a document).
        ///     - catch Ctrl + T (Open/close new tableviewer).
        /// </summary>
        /// <param name="msg">
        ///     A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32
        ///     message to process.
        /// </param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>
        ///     <see langword="true" /> if the keystroke was processed and consumed by the control; otherwise,
        ///     <see langword="false" /> to allow further processing.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.D:
                    mainForm.OpenDocumentPopup();
                    return true;
                case Keys.Control | Keys.T:
                    mainForm.ToggleFloatTableViewer();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}