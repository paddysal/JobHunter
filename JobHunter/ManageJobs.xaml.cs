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
            db_path = db_path.Substring(0, Math.Min(db_path.Length, db_path.Length - 20)) + "Database Files\\JobApplications.accdb";
            //con.ConnectionString = "Provider=Microsoft.Jet.Oledb.12.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\EmpDB.mdb";
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\JobApplications.accdb";
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
                    cmd.CommandText = "INSERT INTO ApplicationHistory(Job Title,Company,via Agency?,Agency Name,Job Url,CV,Cover Letter,Date Applied,Received Reply,Status,Category) Values(" 
                        + txtJobTitle.Text +  "','" + txtCompany.Text + "'," + chkAgency.IsChecked.Value + "'," + txtAgencyName.Text + ",'" + txtJobUrl.Text + ",'" + "'," + chkCV.IsChecked.Value + "'," 
                        + "'," + chkCL.IsChecked.Value + "'," + txtDateApplied.Text + ",'" + "'," + chkReply.IsChecked.Value + "'," + txtStatus.Text +  ",'" + txtCategory.Text + "')";
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Employee Added Successfully...");
                    ClearAll();
                }
                else
                {
                    cmd.CommandText = "UPDATE ApplicationHistory SET JobTitle='" + txtJobTitle.Text + "',Company='" + txtCompany.Text + "',via Agency?='" + chkAgency.IsChecked.Value + "',Agency Name='" + txtAgencyName.Text + 
                        "',Job Url='" + txtJobUrl.Text + "',CV='" + chkCV.IsChecked.Value + "',Cover Letter='" + chkCL.IsChecked.Value + "',Date Applied=" + txtDateApplied.Text + ",Received Reply='" + chkReply.IsChecked.Value + ",Status='" + txtStatus.Text + "' where EmpId=" + txtJobID.Text;
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
            txtDateApplied.Text = "";
            txtStatus.Text = "";
        }

        //Edit records
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtJobTitle.Text = row["Job Title"].ToString();
                chkAgency = row["via Agency?"] as CheckBox;
                txtCompany.Text = row["Company"].ToString();
                txtAgencyName.Text = row["Agency Name"].ToString();
                txtJobUrl.Text = row["Job Url"].ToString();
                txtDateApplied.Text = row["Date Applied"].ToString();
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
    }
}