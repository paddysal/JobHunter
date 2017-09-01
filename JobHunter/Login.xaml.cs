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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String username = txtUsername.Text;
            String password = txtPassword.Password.ToString();
            
             if (username.Length > 5 && password.Length >5)
            {
                //MainWindow main = new MainWindow();
                //App.Current.MainWindow = main;
                //this.Close();
                //main.Show();
                Main_Menu m_menu = new Main_Menu();
                this.Close();
                m_menu.Show();
            }
        }
    }
}
