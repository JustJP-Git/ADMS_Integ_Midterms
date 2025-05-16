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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADMS_Integ_Midterms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (user_txt.Text == "" || pass_txt.Text == "")
            {
                MessageBox.Show("Please ensure that all necessary information has been inputted.");
                return;
            }
            else
            {
                if (passComparison(getPassword()) == 0)
                {
                    Window home;
                    if (getAccessLevel() == "adminaccess")
                    {
                        home = new HomeAdminPrompt();
                        MessageBox.Show("Login successful", $"Welcome back, {user_txt.Text}!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if (getAccessLevel() == "librarianaccess")
                    {
                        home = new HomeLibrarian();
                    }
                    else if (getAccessLevel() == "clericalaccess")
                    {
                        home = new HomeClerical();
                    }
                    else if (getAccessLevel() == "useraccess")
                    {
                        home = new HomeUser();
                    }
                    else if (getAccessLevel() == "studentaccess")
                    {
                        home = new HomeStudentLogin();
                    }
                    else
                    {
                        home = new HomeUser();
                    }

                    home.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private string getPassword()
        {
            string uPass = "";
            var users = (from u in db.Users_Tables
                         where u.User_ID == user_txt.Text
                         select u).FirstOrDefault();

            if (users == null)
            {
                MessageBox.Show("User not found.");
                return "";
            }

            uPass = users.User_Pass;
            return uPass;
        }
        private string getAccessLevel()
        {
            string uLevel = "";
            var users = (from u in db.Users_Tables
                         where u.User_ID == user_txt.Text
                         select u).FirstOrDefault();

            if (users == null)
            {
                MessageBox.Show("User not found.");
                return "";
            }

            uLevel = users.Access_Level;
            return uLevel;
        }

        private int passComparison(string uPass)
        {
            if (pass_txt.Text == uPass)
            {
                return 0; //matched username and password
            }
            else if (uPass == null)
            {
                return -1; //matched password but not username
            }
            else
            {
                return 1; //matched username but not password
            }
        }
    }
}
