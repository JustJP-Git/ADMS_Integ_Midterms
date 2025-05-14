using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for HomeStudent.xaml
    /// </summary>
    public partial class HomeStudent : Window
    {
        public string loginID { get; set; }
        DataClasses1DataContext db1 = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        public HomeStudent()
        {
            InitializeComponent();
            HomeStudentLogin login = new HomeStudentLogin();
            loginID = login.studentid_txt.Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginID))
            {
                MessageBox.Show("No login ID found.");
            }
            else
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                lbStudent.Content = $"Welcome, {loginID}!";

                var users = from u in db1.StudentInfos where u.Student_ID == loginID select u;
                dgStudents.ItemsSource = users.ToList();

                var borrowinfo = from u in db1.BorrowTransactionViews where u.Student_ID == loginID select u;
                dgBorrow.ItemsSource = borrowinfo.ToList();

                var availability = from u in db1.BooksAvailabilityViews select u;
                dgAvail.ItemsSource = availability.ToList();

                var fines = from u in db1.OutstandingFines where u.Student_ID == loginID select u;
                dgFines.ItemsSource = fines.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}