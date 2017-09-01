using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace JobHunter
{
    /// <summary>
    /// Interaction logic for ManageJobs.xaml
    /// </summary>
    public partial class ManageJobs : Window
    {
        private OleDbConnection con;
        private DataTable dt;

        public ManageJobs()
        {
            InitializeComponent();
            //Connect your access database
            con = new OleDbConnection();
            string db_path = AppDomain.CurrentDomain.BaseDirectory;
            string db_path1 = AppDomain.CurrentDomain.BaseDirectory;
            db_path = db_path.Substring(0, Math.Min(db_path.Length, db_path.Length - 20)) + "Database Files\\JobApplications.accdb";
            db_path1 = db_path.Substring(0, Math.Min(db_path.Length, db_path.Length - 36)) + "JobHunter\\JobApplications.accdb";
            MessageBox.Show(db_path1);
            //con.ConnectionString = "Provider=Microsoft.Jet.Oledb.12.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\EmpDB.mdb";
            //con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\JobApplications.accdb";
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + db_path1;
            
            BindGrid();
        }

        //Display records in grid
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from ApplicationHistory";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lblCount.Visibility = Visibility.Hidden;
                gvData.Visibility = Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = Visibility.Visible;
                gvData.Visibility = Visibility.Hidden;
            }
        }

        //Add records in grid
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtJobTitle.Text != "")
            {
                if (btnAdd.Content.Equals("Add"))
                {
                    cmd.CommandText = "INSERT INTO ApplicationHistory(job_title, company, agency, agency_name, job_url, cv_attachment, cover_letter_attachment, date_applied, reply_received, status, category) VALUES('"
                        + txtJobTitle.Text + "','" + txtCompany.Text + "'," + chkAgency.IsChecked.Value + ",'" + txtAgencyName.Text + "','" + txtJobUrl.Text + "'," + chkCV.IsChecked.Value
                        + "," + chkCL.IsChecked.Value + ",'" + dateApplied.SelectedDate + "'," + chkReply.IsChecked.Value + ",'" + txtStatus.Text + "','" + txtCategory.Text + "')";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        string msg = "Error message is: " + ex;
                        MessageBox.Show(msg);
                    }

                    BindGrid();
                    MessageBox.Show("Job Application Added Successfully...");
                    ClearAll();
                }
                else
                {
                    cmd.CommandText = "UPDATE ApplicationHistory SET JobTitle='" + txtJobTitle.Text + "',Company='" + txtCompany.Text + "',Agency='" + chkAgency.IsChecked.Value + "',Agency Name='" + txtAgencyName.Text +
                        "',Job Url='" + txtJobUrl.Text + "',CV='" + chkCV.IsChecked.Value + "',Cover Letter='" + chkCL.IsChecked.Value + "',Date Applied=" + dateApplied.SelectedDate + ",Received Reply='" + chkReply.IsChecked.Value + ",Status='" + txtStatus.Text + "' where EmpId=" + txtJobID.Text;
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Employee Details Updated Succesffully...");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Please Add Job Title.......");
            }
        }

        //Clear all records from controls
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtJobTitle.Text = "";
            txtCategory.Text = "";
            txtCompany.Text = "";
            txtAgencyName.Text = "";
            txtJobUrl.Text = "";
            dateApplied.SelectedDate = null;

            txtStatus.Text = "";
        }

        //Edit records
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtJobTitle.Text = row["Job Title"].ToString();
                chkAgency = row["Agency"] as CheckBox;
                txtCompany.Text = row["Company"].ToString();
                txtAgencyName.Text = row["Agency Name"].ToString();
                txtJobUrl.Text = row["Job Url"].ToString();
                //dateApplied.SelectedDate = row["Date Applied"];
                txtStatus.Text = row["Status"].ToString();
                txtCategory.Text = row["Category"].ToString();
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Please Select Any Job Application From List...");
            }
        }

        //Delete records from grid
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbEmp where EmpId=" + row["EmpId"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Employee Deleted Successfully...");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Please Select Any Employee From List...");
            }
        }

        //Exit
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnNow_Click(object sender, RoutedEventArgs e)
        {
            dateApplied.SelectedDate = DateTime.Today;
        }
    }
}