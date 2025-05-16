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
    /// Interaction logic for HomeStudentPage2.xaml
    /// </summary>
    public partial class HomeStudentPage2 : Page
    {
        DataClasses1DataContext db1 = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        public HomeStudentPage2()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var availability = from u in db1.BooksAvailabilityViews select u;
                dgAvail.ItemsSource = availability.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
