namespace Ra.DocumentInvestigator.OldForWinforms
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Windows.Forms;
    using Leadtools;
    using Leadtools.Codecs;
    using Leadtools.WinForms;

    #endregion

    public class TiffPrint
    {
        #region  Fields

        private int currentPrintPageNumber;

        private PrintDocument printDocument;

        private RasterCodecs rasterCodecs;

        private RasterImage tempImg;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Klasse som indeholder informationer om side i et dokument.
        /// </summary>
        public TiffPrint()
        {
            ImagePrintInit();
        }

        #endregion

        #region

        private void _printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            // Reset sidenummer til første side
            //Her skal forstås side 1 af den indlæste del af flersidet dokument!
            //Hvis side 5-7 ønskes udprintet indlæses disse 3 sider hvorefter side 1-3 udprintes
            currentPrintPageNumber = 1;
        }

        private void _printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //Intet at gøre her ... 
        }

        //************************ Print Image sektion start ****************************
        private void ImagePrintInit()
        {
            // Optional: Unlock support for Document capabilities for ScaleToGray and Bicubic painting -- not supported anymore
            //RasterSupport.Unlock(RasterSupportType.Document, "");

            // Initialize the RasterCodecs object to use when loading image
            //RasterCodecs.Startup();
            rasterCodecs = new RasterCodecs();

            // Check if we have any printers installed
            if (PrinterSettings.InstalledPrinters == null || PrinterSettings.InstalledPrinters.Count < 1)
                throw new NoPrintersFoundException();
            // Create the print document object we will be using
            printDocument = new PrintDocument();
            // Add handlers for the print events
            printDocument.BeginPrint += _printDocument_BeginPrint;
            printDocument.PrintPage += PrintDocumentPrintPage;
            printDocument.EndPrint += _printDocument_EndPrint;
        }

        /// <summary>
        ///     Udprint dokumentfil incl. PrinterDialog
        /// </summary>
        public void PrintDocument(string imageFileName)
        {
            if (File.Exists(imageFileName) == false)
                throw new FileNotFoundException(imageFileName + " ikke fundet!" + "Fejltype 26");

            var info = rasterCodecs.GetInformation(imageFileName, true);

            var pd = new PrintDialog();
            pd.AllowSomePages = true;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = info.TotalPages;
            if (pd.ShowDialog() == DialogResult.OK)
                PrintDocument(imageFileName, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
        }

        /// <summary>
        ///     Udskriv dokument udfra liste af sider fra en eller flere filer (fx mix af tif singlepage og multipage)
        /// </summary>
        /// <param name="pagesList"></param>
        public void PrintDocument(List<DocPage> pagesList, PrintDialog pd)
        {
            //tempImg opbygges udfra de valgte sider
            //Første side indlæses direkte ...
            tempImg = rasterCodecs.Load(
                pagesList[pd.PrinterSettings.FromPage - 1].FileName,
                0,
                CodecsLoadByteOrder.Rgb,
                1,
                1);
            //... evt efterfølgende sider tilføjes ...
            if (pagesList.Count > 1)
                for (var i = pd.PrinterSettings.FromPage; i < pd.PrinterSettings.ToPage; i++)
                    tempImg.InsertPage(
                        -1,
                        rasterCodecs.Load(
                            pagesList[i].FileName,
                            0,
                            CodecsLoadByteOrder.Rgb,
                            pagesList[i].PageNo,
                            pagesList[i].PageNo));

            //Fejlindtastninger håndteres af PrintDialog og i selve udprintningsmetoden
            var dokumentNavn = Path.GetDirectoryName(pagesList[0].FileName); //mappenavn med sti
            dokumentNavn = Path.GetFileName(dokumentNavn); //mappenavn uden sti

            //// Minimum/maximum is the number of pages in the image

            //Default to print all the pages
            printDocument.PrinterSettings.FromPage = 1; // this._printDocument.PrinterSettings.MinimumPage;
            printDocument.PrinterSettings.ToPage = tempImg.PageCount;
            // this._printDocument.PrinterSettings.MaximumPage;

            //Setup the document name
            printDocument.DocumentName = Path.GetFileName(dokumentNavn);

            //Start selve udprintningen af dokumentet (tempImg)
            printDocument.Print();
        }

        /// <summary>
        ///     Udskriv bestemte sider fra dokumentfil
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fromPage"></param>
        /// <param name="toPage"></param>
        public void PrintDocument(string imageFileName, int fromPage, int toPage)
        {
            //if (System.IO.File.Exists(imageFileName) == false)
            //{
            //    MessageBox.Show(imageFileName + " ikke fundet!" + "Fejltype 28");
            //    return;
            //}

            tempImg = rasterCodecs.Load(imageFileName, 0, CodecsLoadByteOrder.Rgb, fromPage, toPage);

            if (tempImg.PageCount > toPage)
                toPage = tempImg.PageCount;

            if (fromPage > tempImg.PageCount)
                fromPage = tempImg.PageCount;

            // Minimum/maximum is the number of pages in the image
            printDocument.PrinterSettings.MinimumPage = 1;
            printDocument.PrinterSettings.MaximumPage = tempImg.PageCount;

            // Default to print all the pages
            printDocument.PrinterSettings.FromPage = 1; // this._printDocument.PrinterSettings.MinimumPage;
            printDocument.PrinterSettings.ToPage = tempImg.PageCount;
            // this._printDocument.PrinterSettings.MaximumPage;

            // Setup the document name
            printDocument.DocumentName = Path.GetFileName(imageFileName);

            //Start printing
            printDocument.Print();
        }

        /// <summary>
        ///     Udskriv dokumentfil incl. PrinterDialog
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="documentName"></param>
        public void PrintDocument(string fileName, string documentName)
        {
            var img = rasterCodecs.Load(fileName, 0, CodecsLoadByteOrder.Rgb, 1, 1);
            var pd = new PrintDialog();
            pd.AllowSomePages = true;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = img.PageCount;

            //Fejlindtastninger håndteres af PrintDialog og i selve udprintningsmetoden
            PrintDocument(img, documentName, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
        }

        /// <summary>
        ///     Udskriv dokumentfil incl. PrinterDialog
        /// </summary>
        /// <param name="img"></param>
        /// <param name="documentName"></param>
        private void PrintDocument(RasterImage img, string documentName)
        {
            var pd = new PrintDialog();
            pd.AllowSomePages = true;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = img.PageCount;
            if (pd.ShowDialog() == DialogResult.OK)
                PrintDocument(img, documentName, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
        }

        /// <summary>
        ///     Udprint bestemte sider fra dokument.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="documentName"></param>
        /// <param name="fromPage"></param>
        /// <param name="toPage"></param>
        private void PrintDocument(RasterImage img, string documentName, int fromPage, int toPage)
        {
            tempImg = new RasterImage(img);

            if (tempImg.PageCount > toPage)
                toPage = tempImg.PageCount;

            if (fromPage > tempImg.PageCount)
                fromPage = tempImg.PageCount;

            // Minimum/maximum is the number of pages in the image
            printDocument.PrinterSettings.MinimumPage = 1;
            printDocument.PrinterSettings.MaximumPage = tempImg.PageCount;

            // Default to print all the pages
            printDocument.PrinterSettings.FromPage = fromPage; // this._printDocument.PrinterSettings.MinimumPage;
            printDocument.PrinterSettings.ToPage = toPage; // this._printDocument.PrinterSettings.MaximumPage;

            currentPrintPageNumber = fromPage;

            // Setup the document name
            printDocument.DocumentName = documentName;

            //Start printing         
            printDocument.Print();
        }

        private void PrintDocumentPrintPage(object sender, PrintPageEventArgs e)
        {
            var document = sender as PrintDocument;

            var printer = new RasterImagePrinter();
            printer.PrintDocument = document;

            //// Check if we want to fit the image
            //if (fitImageToPageToolStripMenuItem.Checked)
            //{
            // Yes, fit and center the image into the maximum print area
            printer.SizeMode = RasterPaintSizeMode.FitAlways;
            printer.HorizontalAlignMode = RasterPaintAlignMode.Center;
            printer.VerticalAlignMode = RasterPaintAlignMode.Center;

            //}
            //else
            //{
            //    // No, print as normal (original size)
            //    printer.SizeMode = RasterPaintSizeMode.Normal;
            //    printer.HorizontalAlignMode = RasterPaintAlignMode.Near;
            //    printer.VerticalAlignMode = RasterPaintAlignMode.Near;
            //}

            // Account for FAX images that may have different horizontal and vertical resolution
            printer.UseDpi = true;

            // Print the whole image
            printer.ImageRectangle = Rectangle.Empty;

            // Use maximum page dimension, this will be equivalant of printing
            // using Windows Photo Gallery
            printer.PageRectangle = RectangleF.Empty;

            //Inform the printer whether we want to ignore the page margins
            printer.UseMargins = false; // usePageMarginsToolStripMenuItem.Checked;

            //*** Print den aktuelle side ****
            tempImg.Page = currentPrintPageNumber; //Sikrer at udprint skaleres til størrelse på siden
            printer.Print(tempImg, currentPrintPageNumber, e);

            // Næste side ...
            currentPrintPageNumber++;

            // Informer printer om vi har flere sider vi ønsker at printe
            if (currentPrintPageNumber <= document.PrinterSettings.ToPage)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

        public void PrintPage(PrintDocument document, PrintPageEventArgs e)
        {
            var printer = new RasterImagePrinter();
            // Assign document object således at diverse settings kan beregnes
            printer.PrintDocument = document;

            printer.SizeMode = RasterPaintSizeMode.FitAlways;
            printer.HorizontalAlignMode = RasterPaintAlignMode.Center;
            printer.VerticalAlignMode = RasterPaintAlignMode.Center;

            // Account for FAX images that may have different horizontal and vertical resolution
            printer.UseDpi = true;

            // Print the whole image
            printer.ImageRectangle = Rectangle.Empty;

            // Use maximum page dimension, this will be equivalant of printing
            // using Windows Photo Gallery
            printer.PageRectangle = RectangleF.Empty;

            //Inform the printer whether we want to ignore the page margins
            printer.UseMargins = false; // usePageMarginsToolStripMenuItem.Checked;

            //*** Print den aktuelle side ****
            tempImg.Page = currentPrintPageNumber; //Sikrer at udprint skaleres til størrelse på siden
            printer.Print(tempImg, currentPrintPageNumber, e);

            // Næste side ...
            currentPrintPageNumber++;

            // Informer printer om vi har flere sider vi ønsker at printe
            if (currentPrintPageNumber <= document.PrinterSettings.ToPage)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

        #endregion
    }
}