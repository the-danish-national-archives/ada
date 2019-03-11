namespace Ada.UI.Winforms.Printing
{
    #region Namespace Using

    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using log4net;

    #endregion

    /// <summary>
    ///     A helper to provide an easy method for printing a RichTextBox
    ///     based of work by Martin Müller http://msdn.microsoft.com/en-us/library/ms996492.aspx
    /// </summary>
    public class RichTextBoxPrintHelper
    {
        #region Static

        // Windows Messages defines
        private const int WM_USER = 0x400;
        private const int EM_FORMATRANGE = WM_USER + 57;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;

        // Defines for EM_SETCHARFORMAT/EM_GETCHARFORMAT
        private const int SCF_SELECTION = 0x0001;
        private const int SCF_WORD = 0x0002;
        private const int SCF_ALL = 0x0004;

        // Defines for STRUCT_CHARFORMAT member dwMask
        private const uint CFM_BOLD = 0x00000001;
        private const uint CFM_ITALIC = 0x00000002;
        private const uint CFM_UNDERLINE = 0x00000004;
        private const uint CFM_STRIKEOUT = 0x00000008;
        private const uint CFM_PROTECTED = 0x00000010;
        private const uint CFM_LINK = 0x00000020;
        private const uint CFM_SIZE = 0x80000000;
        private const uint CFM_COLOR = 0x40000000;
        private const uint CFM_FACE = 0x20000000;
        private const uint CFM_OFFSET = 0x10000000;
        private const uint CFM_CHARSET = 0x08000000;

        // Defines for STRUCT_CHARFORMAT member dwEffects
        private const uint CFE_BOLD = 0x00000001;
        private const uint CFE_ITALIC = 0x00000002;
        private const uint CFE_UNDERLINE = 0x00000004;
        private const uint CFE_STRIKEOUT = 0x00000008;
        private const uint CFE_PROTECTED = 0x00000010;
        private const uint CFE_LINK = 0x00000020;
        private const uint CFE_AUTOCOLOR = 0x40000000;

        #endregion

        #region  Fields

        private readonly RichTextBox control;
        private int currentPageNumber;


        private int m_nFirstCharOnPage;

        #endregion

        #region  Constructors

        public RichTextBoxPrintHelper(RichTextBox controlToPrint)
        {
            control = controlToPrint;
        }

        #endregion

        #region

        /// <summary>
        ///     Calculate or render the contents of our RichTextBox for printing
        /// </summary>
        /// <param name="measureOnly">
        ///     If true, only the calculation is performed,
        ///     otherwise the text is rendered as well
        /// </param>
        /// <param name="e">
        ///     The PrintPageEventArgs object from the
        ///     PrintPage event
        /// </param>
        /// <param name="charFrom">Index of first character to be printed</param>
        /// <param name="charTo">Index of last character to be printed</param>
        /// <returns>
        ///     (Index of last character that fitted on the
        ///     page) + 1
        /// </returns>
        public int FormatRange
        (bool measureOnly, PrintPageEventArgs e,
            int charFrom, int charTo)
        {
            // Specify which characters to print
            STRUCT_CHARRANGE cr;
            cr.cpMin = charFrom;
            cr.cpMax = charTo;

            // Specify the area inside page margins
            STRUCT_RECT rc;
            rc.top = HundredthInchToTwips(e.MarginBounds.Top);
            rc.bottom = HundredthInchToTwips(e.MarginBounds.Bottom);
            rc.left = HundredthInchToTwips(e.MarginBounds.Left);
            rc.right = HundredthInchToTwips(e.MarginBounds.Right);

            // Specify the page area
            STRUCT_RECT rcPage;
            rcPage.top = HundredthInchToTwips(e.PageBounds.Top);
            rcPage.bottom = HundredthInchToTwips(e.PageBounds.Bottom);
            rcPage.left = HundredthInchToTwips(e.PageBounds.Left);
            rcPage.right = HundredthInchToTwips(e.PageBounds.Right);

            // Get device context of output device
            var hdc = e.Graphics.GetHdc();

            // Fill in the FORMATRANGE struct
            STRUCT_FORMATRANGE fr;
            fr.chrg = cr;
            fr.hdc = hdc;
            fr.hdcTarget = hdc;
            fr.rc = rc;
            fr.rcPage = rcPage;

            // Non-Zero wParam means render, Zero means measure
            var wParam = measureOnly ? 0 : 1;

            // Allocate memory for the FORMATRANGE struct and
            // copy the contents of our struct to this memory
            var lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
            Marshal.StructureToPtr(fr, lParam, false);

            // Send the actual Win32 message
            var res = SendMessage(control.Handle, EM_FORMATRANGE, wParam, lParam);

            // Free allocated memory
            Marshal.FreeCoTaskMem(lParam);

            // and release the device context
            e.Graphics.ReleaseHdc(hdc);

            return res;
        }

        /// <summary>
        ///     Free cached data from rich edit control after printing
        /// </summary>
        public void FormatRangeDone()
        {
            var lParam = new IntPtr(0);
            SendMessage(control.Handle, EM_FORMATRANGE, 0, lParam);
        }

        /// <summary>
        ///     Convert between 1/100 inch (unit used by the .NET framework)
        ///     and twips (1/1440 inch, used by Win32 API calls)
        /// </summary>
        /// <param name="n">Value in 1/100 inch</param>
        /// <returns>Value in twips</returns>
        private int HundredthInchToTwips(int n)
        {
            return (int) (n * 14.4);
        }

        private void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            // Start at the beginning of the text
            m_nFirstCharOnPage = 0;

            currentPageNumber = 1;
        }

        private void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            // Clean up cached information
            FormatRangeDone();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // To print the boundaries of the current page margins
            // uncomment the next line:
            //e.Graphics.DrawRectangle(System.Drawing.Pens.Blue, e.MarginBounds);

            //Tilføj sidenummer
            var b = Brushes.Black;
            var f = new Font("Courier New", 11.0F);
            var sideNr = "Side " + currentPageNumber;

            var sideNrSize = TextRenderer.MeasureText(sideNr, f);
            var textXOffSet = (e.PageBounds.Width - sideNrSize.Width) / 2; // Convert.ToInt32((e.MarginBounds.Width - sideNrWidth) / 2);

            //Sidenummer placeres 1 linie under indholdstekst
            e.Graphics.DrawString(sideNr, f, b, textXOffSet, e.MarginBounds.Bottom + sideNrSize.Height);


            // make the RichTextBoxEx calculate and render as much text as will
            // fit on the page and remember the last character printed for the
            // beginning of the next page
            m_nFirstCharOnPage = FormatRange(false, e, m_nFirstCharOnPage, control.TextLength);

            // check if there are more pages to print
            if (m_nFirstCharOnPage < control.TextLength)
            {
                e.HasMorePages = true;
                currentPageNumber++;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        public void PrintRTF()
        {
            PrintRTF(new PrintDocument());
        }


        public void PrintRTF(PrintDocument printDocument)
        {
            try
            {
                printDocument.BeginPrint += printDocument_BeginPrint;
                printDocument.EndPrint += printDocument_EndPrint;
                printDocument.PrintPage += printDocument_PrintPage;
                printDocument.Print();
            }
            catch (Exception ex)
            {
                var Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

                if (Log.IsDebugEnabled) Log.Debug("Exception in PrintRTF: " + ex.Message);
            }
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage
        (IntPtr hWnd, int msg,
            int wParam, IntPtr lParam);

        /// <summary>
        ///     Sets the bold style only for the selected part of the rich text box
        ///     without modifying the other properties like font or size
        /// </summary>
        /// <param name="bold">make selection bold (true) or regular (false)</param>
        /// <returns>true on success, false on failure</returns>
        public bool SetSelectionBold(bool bold)
        {
            return SetSelectionStyle(CFM_BOLD, bold ? CFE_BOLD : 0);
        }

        /// <summary>
        ///     Sets the font only for the selected part of the rich text box
        ///     without modifying the other properties like size or style
        /// </summary>
        /// <param name="face">Name of the font to use</param>
        /// <returns>true on success, false on failure</returns>
        public static bool SetSelectionFont(RichTextBox control, string face)
        {
            var cf = new CHARFORMATSTRUCT();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];
            cf.dwMask = CFM_FACE;
            face.CopyTo(0, cf.szFaceName, 0, Math.Min(31, face.Length));

            var lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lParam, false);

            var res = SendMessage(control.Handle, EM_SETCHARFORMAT, SCF_SELECTION, lParam);
            return res == 0;
        }

        /// <summary>
        ///     Sets the italic style only for the selected part of the rich text box
        ///     without modifying the other properties like font or size
        /// </summary>
        /// <param name="italic">make selection italic (true) or regular (false)</param>
        /// <returns>true on success, false on failure</returns>
        public bool SetSelectionItalic(bool italic)
        {
            return SetSelectionStyle(CFM_ITALIC, italic ? CFE_ITALIC : 0);
        }

        /// <summary>
        ///     Sets the font size only for the selected part of the rich text box
        ///     without modifying the other properties like font or style
        /// </summary>
        /// <param name="size">new point size to use</param>
        /// <returns>true on success, false on failure</returns>
        public static bool SetSelectionSize(RichTextBox control, int size)
        {
            var cf = new CHARFORMATSTRUCT();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.dwMask = CFM_SIZE;
            // yHeight is in 1/20 pt
            cf.yHeight = size * 20;

            var lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lParam, false);

            var res = SendMessage(control.Handle, EM_SETCHARFORMAT, SCF_SELECTION, lParam);
            return res == 0;
        }

        /// <summary>
        ///     Set the style only for the selected part of the rich text box
        ///     with the possibility to mask out some styles that are not to be modified
        /// </summary>
        /// <param name="mask">modify which styles?</param>
        /// <param name="effect">new values for the styles</param>
        /// <returns>true on success, false on failure</returns>
        private bool SetSelectionStyle(uint mask, uint effect)
        {
            var cf = new CHARFORMATSTRUCT();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;

            var lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lParam, false);

            var res = SendMessage(control.Handle, EM_SETCHARFORMAT, SCF_SELECTION, lParam);
            return res == 0;
        }

        /// <summary>
        ///     Sets the underlined style only for the selected part of the rich text box
        ///     without modifying the other properties like font or size
        /// </summary>
        /// <param name="underlined">make selection underlined (true) or regular (false)</param>
        /// <returns>true on success, false on failure</returns>
        public bool SetSelectionUnderlined(bool underlined)
        {
            return SetSelectionStyle(CFM_UNDERLINE, underlined ? CFE_UNDERLINE : 0);
        }

        #endregion

        #region Nested type: CHARFORMATSTRUCT

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMATSTRUCT
        {
            public int cbSize;
            public uint dwMask;
            public uint dwEffects;
            public int yHeight;
            public readonly int yOffset;
            public readonly int crTextColor;
            public readonly byte bCharSet;
            public readonly byte bPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
        }

        #endregion

        #region Nested type: STRUCT_CHARRANGE

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_CHARRANGE
        {
            public int cpMin;
            public int cpMax;
        }

        #endregion

        #region Nested type: STRUCT_FORMATRANGE

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_FORMATRANGE
        {
            public IntPtr hdc;
            public IntPtr hdcTarget;
            public STRUCT_RECT rc;
            public STRUCT_RECT rcPage;
            public STRUCT_CHARRANGE chrg;
        }

        #endregion

        #region Nested type: STRUCT_RECT

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion
    }
}