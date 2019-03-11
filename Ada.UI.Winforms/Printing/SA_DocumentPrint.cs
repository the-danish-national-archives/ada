namespace Ada.UI.Winforms.Printing
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Windows.Forms;
    using Ra.DocumentInvestigator.OldForWinforms;

    #endregion

    public class SA_DocumentPrint
    {
        #region  Fields

        //*** ImagePrint ***
        //RasterImage tempImg;
        //private RasterCodecs _rasterCodecs;
        private readonly TiffPrint tiffPrint = new TiffPrint();

        //PrintDocument object
        private PrintDocument _printDocument;

        #endregion

        #region  Constructors

//        //Sidenummer som skal printes
//        private int _currentPrintPageNumber;


        /// <summary>
        ///     Klasse som indeholder informationer om side i et dokument.
        /// </summary>
        //public class docPage
        //{
        //    public string fileName;
        //    public int pageNo;
        //}

        //Constructor
        public SA_DocumentPrint()
        {
            //Initialisering af ImagePrint
            ImagePrintInit();
        }

        #endregion

        #region

        private void _printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            // Reset sidenummer til første side
            //Her skal forstås side 1 af den indlæste del af flersidet dokument!
            //Hvis side 5-7 ønskes udprintet indlæses disse 3 sider hvorefter side 1-3 udprintes
//            this._currentPrintPageNumber = 1;
        }


        private void _printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //Intet at gøre her ... 
        }


        private void _printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Print a page here

            // Print document object
            var document = sender as PrintDocument;
            tiffPrint.PrintPage(document, e);

            //var printer = TiffLeadToolsProcessor.GetPrinter();
            //// Assign document object således at diverse settings kan beregnes
            //printer.PrintDocument = document;


            //TiffLeadToolsProcessor.SetRasterPrinterOptions(printer);


            //// Account for FAX images that may have different horizontal and vertical resolution
            //printer.UseDpi = true;

            //// Print the whole image
            //printer.ImageRectangle = Rectangle.Empty;

            //// Use maximum page dimension, this will be equivalant of printing
            //// using Windows Photo Gallery
            //printer.PageRectangle = RectangleF.Empty;

            ////Inform the printer whether we want to ignore the page margins
            //printer.UseMargins = false;// usePageMarginsToolStripMenuItem.Checked;

            ////*** Print den aktuelle side ****
            //tempImg.Page = _currentPrintPageNumber; //Sikrer at udprint skaleres til størrelse på siden
            //printer.Print(tempImg, _currentPrintPageNumber, e);

            //// Næste side ...
            //this._currentPrintPageNumber++;

            //// Informer printer om vi har flere sider vi ønsker at printe
            //if (this._currentPrintPageNumber <= document.PrinterSettings.ToPage)
            //{
            //    e.HasMorePages = true;
            //}
            //else
            //{
            //    e.HasMorePages = false;
            //}
        }


        private void Dispose()
        {
            // Dispose the resources we are using
            if (_printDocument != null) _printDocument.Dispose();

            //if (this._rasterCodecs != null)
            //{
            //    this._rasterCodecs.Dispose();
            //}
            //RasterCodecs.Shutdown();
        }

        private static PrintDialog GetDefaultPrintDialog(int noOffPages)
        {
            var pd = new PrintDialog();
            pd.AllowSomePages = true;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = noOffPages;
            return pd;
        }


        //************************ Print Image sektion start ****************************
        private void ImagePrintInit()
        {
            // Optional: Unlock support for Document capabilities for ScaleToGray and Bicubic painting -- not supported anymore


            // Initialize the RasterCodecs object to use when loading image
            // this._rasterCodecs = new RasterCodecs();

            // Check if we have any printers installed
            if (PrinterSettings.InstalledPrinters == null || PrinterSettings.InstalledPrinters.Count < 1)
            {
                MessageBox.Show(
                    "No printer installed on this machine, cannot continue",
                    "Print Image",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else
            {
                // Create the print document object we will be using
                _printDocument = new PrintDocument();
                // Add handlers for the print events
                _printDocument.BeginPrint += _printDocument_BeginPrint;
                _printDocument.PrintPage += _printDocument_PrintPage;
                _printDocument.EndPrint += _printDocument_EndPrint;
            }
        }


        /// <summary>
        ///     Udprint dokumentfil incl. PrinterDialog
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintDocument(string imageFileName)
        {
            if (File.Exists(imageFileName) == false)
            {
                MessageBox.Show(string.Format("{0} ikke fundet! Fejltype 26", imageFileName));
                return;
            }

            var tiffPrint = new TiffPrint();

            var pd = GetDefaultPrintDialog(new TiffLeadToolsProcessor().GetPageCount(imageFileName));
            if (pd.ShowDialog() == DialogResult.OK) tiffPrint.PrintDocument(imageFileName, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
        }


        /// <summary>
        ///     Udskriv dokument udfra liste af sider fra en eller flere filer (fx mix af tif singlepage og multipage)
        /// </summary>
        /// <param name="pagesList"></param>
        public void PrintDocument(List<DocPage> pagesList)
        {
            var pd = GetDefaultPrintDialog(pagesList.Count);
            if (
                pd.ShowDialog(new Form {TopMost = true}) // new form to get dialog on top
                == DialogResult.OK)
            {
                //Først en test af om filer findes ...
                foreach (var page in pagesList)
                    if (File.Exists(page.FileName) == false)
                    {
                        MessageBox.Show(string.Format("{0} ikke fundet!Fejltype 27", page.FileName));
                        return;
                    }

                tiffPrint.PrintDocument(pagesList, pd);


                ////tempImg opbygges udfra de valgte sider
                ////Første side indlæses direkte ...
                //tempImg = _rasterCodecs.Load(pagesList[pd.PrinterSettings.FromPage-1].fileName, 0, CodecsLoadByteOrder.Rgb, 1, 1);
                ////... evt efterfølgende sider tilføjes ...
                //if (pagesList.Count > 1)
                //{
                //    //Bemærk +1 fordi første side allerede er tilføjet
                //    //for (int i = pd.PrinterSettings.FromPage + 1; i <= pd.PrinterSettings.ToPage; i++)
                //    for (int i = pd.PrinterSettings.FromPage; i < pd.PrinterSettings.ToPage; i++)
                //    {
                //        //tempImg = _rasterCodecs.Load(imageFileName, 0, CodecsLoadByteOrder.Rgb, fromPage, toPage); 
                //        tempImg.InsertPage(-1, _rasterCodecs.Load(pagesList[i].fileName, 0, CodecsLoadByteOrder.Rgb, pagesList[i].pageNo, pagesList[i].pageNo));
                //    }
                //}

                ////Fejlindtastninger håndteres af PrintDialog og i selve udprintningsmetoden
                //string dokumentNavn = System.IO.Path.GetDirectoryName(pagesList[0].fileName);//mappenavn med sti
                //dokumentNavn = System.IO.Path.GetFileName(dokumentNavn);//mappenavn uden sti

                ////// Minimum/maximum is the number of pages in the image


                ////Default to print all the pages
                //this._printDocument.PrinterSettings.FromPage = 1;// this._printDocument.PrinterSettings.MinimumPage;
                //this._printDocument.PrinterSettings.ToPage = tempImg.PageCount;// this._printDocument.PrinterSettings.MaximumPage;


                ////Setup the document name
                //this._printDocument.DocumentName = System.IO.Path.GetFileName(dokumentNavn);

                ////Start selve udprintningen af dokumentet (tempImg)
                //this._printDocument.Print();
            }
        }

        ///// <summary>
        ///// Udskriv dokumentfil incl. PrinterDialog
        ///// </summary>
        ///// <param name="img"></param>
        ///// <param name="DocumentName"></param>
        //public void PrintDocument(RasterImage img, string DocumentName)
        //{

        //    PrintDialog pd = GetDefaultPrintDialog(9999);
        //    if (pd.ShowDialog() == DialogResult.OK)
        //    {
        //        //Fejlindtastninger håndteres af PrintDialog og i selve udprintningsmetoden
        //       // PrintDocument(img, DocumentName, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);

        //    }

        //}

        ///// <summary>
        ///// Udprint bestemte sider fra dokument.
        ///// </summary>
        ///// <param name="img"></param>
        ///// <param name="DocumentName"></param>
        ///// <param name="fromPage"></param>
        ///// <param name="toPage"></param>
        //private void PrintDocument(RasterImage img, string DocumentName,int fromPage, int toPage)
        //{
        //    tempImg = new RasterImage(img);

        //    if (tempImg.PageCount > toPage)
        //    {
        //        toPage = tempImg.PageCount;
        //    }

        //    if (fromPage > tempImg.PageCount)
        //    {
        //        fromPage = tempImg.PageCount;
        //    }

        //    // Minimum/maximum is the number of pages in the image
        //    this._printDocument.PrinterSettings.MinimumPage = 1;
        //    this._printDocument.PrinterSettings.MaximumPage = tempImg.PageCount;

        //    // Default to print all the pages
        //    this._printDocument.PrinterSettings.FromPage = fromPage;// this._printDocument.PrinterSettings.MinimumPage;
        //    this._printDocument.PrinterSettings.ToPage = toPage;// this._printDocument.PrinterSettings.MaximumPage;

        //    this._currentPrintPageNumber = fromPage;

        //    // Setup the document name
        //    this._printDocument.DocumentName = DocumentName;

        //    //Start printing         
        //    this._printDocument.Print();
        //}


        //************************ RichText print  ****************************
        /// <summary>
        ///     Udprint tekst fra RichTextBox.
        ///     Alle styles, markeringer etc. bibeholdes.
        /// </summary>
        /// <param name="control"></param>
        public void PrintTekst(RichTextBox control)
        {
            var helper = new RichTextBoxPrintHelper(control);
            helper.PrintRTF();
        }


        //************************ Tekst print  ****************************
        /// <summary>
        ///     Udprint tekst med angivelse af font navn og størrelse.
        ///     Fx printTekst(richTextBox1.Lines,"Courier New", 12.0F)
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        public void PrintTekst(string[] stringArray, string fontName, float fontSize)
        {
            var control = new RichTextBox();
            control.Font = new Font(fontName, fontSize);
            control.Lines = stringArray;
            var helper = new RichTextBoxPrintHelper(control);
            helper.PrintRTF();
        }

        #endregion
    }


    //************************ RichTextBoxPrintHelper klasse **************************** 
}