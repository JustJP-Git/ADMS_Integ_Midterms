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
    /// Interaction logic for HomeAdminPrompt.xaml
    /// </summary>
    public partial class HomeAdminPrompt : Window
    {
        public HomeAdminPrompt()
        {
            InitializeComponent();
        }

        private void btnStudents_Click(object sender, RoutedEventArgs e)
        {
            HomeAdminStudents students = new HomeAdminStudents();
            students.Show();
            this.Close();
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            HomeAdmin users = new HomeAdmin();
            users.Show();
            this.Close();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
