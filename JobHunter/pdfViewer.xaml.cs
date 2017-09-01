using PdfPrintingNet;
using PdfViewerNet;
using System.Windows;

namespace JobHunter
{
    /// <summary>
    /// Interaction logic for pdfViewer.xaml
    /// </summary>
    public partial class pdfViewer : Window
    {
        public pdfViewer()
        {
            InitializeComponent();

            var pdfViewer = new PdfViewer();

            pdfViewer.DocumentLoaded += PdfViewer_DocumentLoaded;

            PdfOpenFileStatus status = pdfViewer.OpenDocument(@"c:\JobHunter\CVs\cv.pdf");

            string msg = "";

            if (status.Result == PdfOpenFileStatus.PdfOpenFileResult.OK)
            {
                msg = string.Format("Document has {0} pages. Current page is {1}. ", pdfViewer.NumberOfPages, pdfViewer.CurrentPageNumber);
            }
            else
            {
                msg = "There was a problem loading document. Status = " + status.Result.ToString() + "Additional information = " + status.Status;
                MessageBox.Show(msg);
            }
        }


        private void PdfViewer_DocumentLoaded(object sender, DocumentLoadedEventArgs e)

        {
            MessageBox.Show("Document " + e.FileName + " was loaded");
        }
    }
}