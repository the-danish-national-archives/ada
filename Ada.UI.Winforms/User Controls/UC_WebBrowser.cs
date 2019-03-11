namespace Ada.UI.Winforms.User_Controls
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    #endregion

    public partial class UC_WebBrowser : UserControl
    {
        #region  Fields

        private Dictionary<string, string> UrlList = new Dictionary<string, string>();

        #endregion

        #region  Constructors

        //UrlList.Add("Bekendtgørelse", "www.sa.dk");
        //UrlList.Add("Google", "www.google.dk");


        public UC_WebBrowser(Dictionary<string, string> tmpUrlList)
        {
            InitializeComponent();

            UrlList = tmpUrlList;

            comboBox1.Items.Clear();
            foreach (var url in UrlList) comboBox1.Items.Add(url.Key);

            comboBox1.SelectedIndex = 0;

            LoadPageWithValidation(UrlList.ElementAt(comboBox1.SelectedIndex).Value);
        }


        public UC_WebBrowser(Dictionary<string, string> tmpUrlList, bool showAddress)
        {
            InitializeComponent();


            textBox1.Visible = showAddress;
            UrlList = tmpUrlList;

            comboBox1.Items.Clear();
            foreach (var url in UrlList) comboBox1.Items.Add(url.Key);

            comboBox1.SelectedIndex = 0;

            LoadPageWithValidation(UrlList.ElementAt(comboBox1.SelectedIndex).Value);
        }


        public UC_WebBrowser()
        {
            InitializeComponent();
        }


        public UC_WebBrowser(string path)
        {
            InitializeComponent();
            LoadPageWithValidation(path);
        }

        #endregion

        #region

        private void BrowserGoBackButton_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoBack) webBrowser1.GoBack();
        }

        private void BrowserGoForwardButton_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoForward) webBrowser1.GoForward();
        }

        private void BrowserPrintButton_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPageWithValidation(UrlList.ElementAt(comboBox1.SelectedIndex).Value);
        }


        public void LoadLinkList(Dictionary<string, string> tmpUrlList)
        {
            UrlList = tmpUrlList;

            comboBox1.Items.Clear();
            foreach (var url in UrlList) comboBox1.Items.Add(url.Key);
            comboBox1.SelectedIndex = 0;

            LoadPageWithValidation(UrlList.ElementAt(0).Value);
        }


        public void LoadLinkList(Dictionary<string, string> tmpUrlList, bool showAddress)
        {
            textBox1.Visible = showAddress;

            UrlList = tmpUrlList;

            comboBox1.Items.Clear();
            foreach (var url in UrlList) comboBox1.Items.Add(url.Key);
            comboBox1.SelectedIndex = 0;

            LoadPageWithValidation(UrlList.ElementAt(0).Value);
        }


        public void LoadPage(string path)
        {
            LoadPageWithValidation(path);
        }


        private void LoadPageWithValidation(string addr)
        {
            if (addr.ToLower().StartsWith("http://"))
                webBrowser1.Url = new Uri(addr);
            else if (addr.ToLower().StartsWith("file://"))
                webBrowser1.Url = new Uri(addr);
            else if (addr.ToLower().StartsWith("ftp://"))
                webBrowser1.Url = new Uri(addr);
            else if (addr.ToLower().Contains(@":\"))
                webBrowser1.Url = new Uri(addr);
            else if (addr.ToLower().StartsWith(@"\\"))
                webBrowser1.Url = new Uri(addr);
            else
                webBrowser1.Url = new Uri("http://" + addr);


            webBrowser1.Focus(); //Klar til til at scrolle etc i webbrowser
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //enter
                LoadPageWithValidation(textBox1.Text);
        }


        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();
        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            BrowserGoForwardButton.Enabled = webBrowser1.CanGoForward;
            BrowserGoBackButton.Enabled = webBrowser1.CanGoBack;
            textBox1.Text = webBrowser1.Url.ToString();
        }

        #endregion
    }
}