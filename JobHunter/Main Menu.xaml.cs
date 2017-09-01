using System;
using System.IO;
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
    /// Interaction logic for Main_Menu.xaml
    /// </summary>
    public partial class Main_Menu : Window
    {
        private int CvFile_count = 0;
        private int ClFile_count = 0;

        public void getCounts()
        {
            //Specify directory path to manipulate.
            string pathCVs = @"/JobHunter/CVs/";
            string pathCLs = @"/JobHunter/Cover Letters/";

            //Create directories for cvs and cover letters if they don't exist already
            System.IO.Directory.CreateDirectory(pathCVs);
            System.IO.Directory.CreateDirectory(pathCLs);

            string searchPattern = "t*";

            DirectoryInfo cv_di = new DirectoryInfo(pathCVs);
            DirectoryInfo cl_di = new DirectoryInfo(pathCLs);
            DirectoryInfo[] cv_sub_directories = cv_di.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);
            DirectoryInfo[] cl_sub_directories = cl_di.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);

            FileInfo[] cv_files = cv_di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
            FileInfo[] cl_files = cl_di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

            Console.WriteLine("Directories that begin with the letter \"c\" in {0}", pathCVs);
            foreach (DirectoryInfo dir in cv_sub_directories)
            {
                Console.WriteLine(
                    "{0,-25} {1,25}", dir.FullName, dir.LastWriteTime);
            }

            foreach (DirectoryInfo dir in cl_sub_directories)
            {
                Console.WriteLine(
                    "{0,-25} {1,25}", dir.FullName, dir.LastWriteTime);
            }

            Console.WriteLine("Files that begin with the letter \"c\" in {0}", pathCVs);
            foreach (FileInfo file in cv_files)
            {
                CvFile_count += 1;
                ListBoxItem itm = new ListBoxItem();
                itm.Content = file.Name;

                lbxCVs.Items.Add(itm);
                Console.WriteLine(
                    "{0,-25} {1,25}", file.Name, file.LastWriteTime);
            }
            lblCVsNo.Content = CvFile_count;

            Console.WriteLine("Files that begin with the letter \"c\" in {0}", pathCLs);
            foreach (FileInfo file in cl_files)
            {
                ClFile_count += 1;
                ListBoxItem itm = new ListBoxItem();
                itm.Content = file.Name;

                lbxCLs.Items.Add(itm);
                Console.WriteLine(
                    "{0,-25} {1,25}", file.Name, file.LastWriteTime);
            }
            lblCLsNo.Content = ClFile_count;
            string db_path = AppDomain.CurrentDomain.BaseDirectory;
            lblAppliedCount.Content = db_path.Substring(0, Math.Min(db_path.Length, db_path.Length - 20)) + "Database Files\\JobApplications.accdb";

        }

        public Main_Menu()
        {
            InitializeComponent();
            getCounts();
        }

        private void btnAppHistory_Click(object sender, RoutedEventArgs e)
        {
            History history = new History();
            this.Close();
            history.Show();
        }

        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            ManageJobs manage_jobs = new ManageJobs();
            this.Close();
            manage_jobs.Show();
        }
    }
}
