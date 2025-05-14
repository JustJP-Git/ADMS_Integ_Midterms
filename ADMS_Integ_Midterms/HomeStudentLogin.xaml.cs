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

namespace ADMS_Integ_Midterms
{
    /// <summary>
    /// Interaction logic for HomeStudentLogin.xaml
    /// </summary>
    public partial class HomeStudentLogin : Window
    {
        public HomeStudentLogin()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(studentid_txt.Text))
            {
                MessageBox.Show("Please input your student ID.");
            }
            else
            {
                MessageBox.Show("Login successful", $"Welcome back, {studentid_txt.Text}!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                HomeStudent home = new HomeStudent();
                home.loginID = studentid_txt.Text;
                home.Show();
                this.Close();
            }
        }
    }
}
