using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JobHunter
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : Window
    {
        public History()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            JobHunter.JobApplicationsDataSet jobApplicationsDataSet = ((JobHunter.JobApplicationsDataSet)(this.FindResource("jobApplicationsDataSet")));
            // Load data into the table ApplicationHistory. You can modify this code as needed.
            JobHunter.JobApplicationsDataSetTableAdapters.ApplicationHistoryTableAdapter jobApplicationsDataSetApplicationHistoryTableAdapter = new JobHunter.JobApplicationsDataSetTableAdapters.ApplicationHistoryTableAdapter();
            jobApplicationsDataSetApplicationHistoryTableAdapter.Fill(jobApplicationsDataSet.ApplicationHistory);
            System.Windows.Data.CollectionViewSource applicationHistoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("applicationHistoryViewSource")));
            applicationHistoryViewSource.View.MoveCurrentToFirst();
        }
    }
}
