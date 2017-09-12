using System;
using System.Collections;
using System.Collections.Generic;
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
            txtJobID.IsEnabled = false;
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
                    //if (chkAgency.IsChecked.HasValue && chkAgency.IsChecked.Value)
                    //{
                    //    chkAgency.IsChecked = true;
                    //}
                    //else
                    //{
                    //    chkAgency.IsChecked = false;
                    //}
                    cmd.CommandText = "UPDATE ApplicationHistory SET job_title='" + txtJobTitle.Text + "', company='" + txtCompany.Text + "',agency=" + chkAgency.IsChecked.Value + ",agency_name='" + txtAgencyName.Text +
                        "',job_url='" + txtJobUrl.Text + "',cv_attachment=" + chkCV.IsChecked.Value + ",cover_letter_attachment=" + chkCL.IsChecked.Value + ",date_applied='" + dateApplied.SelectedDate + "',reply_received=" + chkReply.IsChecked.Value + ",status='" + txtStatus.Text + "', category='" + txtCategory.Text + "' WHERE ID=" + txtJobID.Text;
                    //UPDATE ApplicationHistory
                    //SET job_title = 'asa', company = 'asa', agency = true, agency_name = 'asa', job_url = 'asa', cv_attachment = true, cover_letter_attachment = true,
                    //                         date_applied = '01/09/2017', reply_received = true, status = 'active', category = 'it'
                    //WHERE(ID = 4)
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
            txtJobID.Text = "";
            txtJobTitle.Text = "";
            txtCompany.Text = "";
            chkAgency.IsChecked = false;
            txtAgencyName.Text = "";
            txtJobUrl.Text = "";
            chkCV.IsChecked = false;
            chkCL.IsChecked = false;
            dateApplied.SelectedDate = null;
            txtStatus.Text = "";
            txtCategory.Text = "";

            txtStatus.Text = "";
        }

        //Edit records
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtJobID.Text = row["ID"].ToString();
                txtJobTitle.Text = row["job_title"].ToString();
                txtCompany.Text = row["company"].ToString();
                if (row["agency"].Equals(true))
                {
                    chkAgency.IsChecked = true;
                } else
                {
                    chkAgency.IsChecked = false;
                }
                txtAgencyName.Text = row["agency_name"].ToString();
                txtJobUrl.Text = row["job_url"].ToString();
                if (row["cv_attachment"].Equals(true))
                {
                    chkCV.IsChecked = true;
                }
                else
                {
                    chkCV.IsChecked = false;
                }
                if (row["cover_letter_attachment"].Equals(true))
                {
                    chkCL.IsChecked = true;
                }
                else
                {
                    chkCL.IsChecked = false;
                }
                DateTime date = Convert.ToDateTime(row["date_applied"].ToString());
                dateApplied.SelectedDate = date;
                if (row["reply_received"].Equals(true))
                {
                    chkReply.IsChecked = true;
                }
                else
                {
                    chkReply.IsChecked = false;
                }
                txtStatus.Text = row["status"].ToString();
                txtCategory.Text = row["category"].ToString();
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
                cmd.CommandText = "delete from ApplicationHistory where ID=" + row["ID"].ToString();
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

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }


        private void gvData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var row_list = GetDataGridRows(gvData);
                foreach (DataGridRow single_row in row_list)
                {
                    if (single_row.IsSelected == true)
                    {
                        MessageBox.Show("the row no." + single_row.GetIndex().ToString() + " is selected!");
                    }
                }

            }
            catch { }
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