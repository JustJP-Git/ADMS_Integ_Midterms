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
    /// Interaction logic for HomeLibrarian.xaml
    /// </summary>
    public partial class HomeLibrarian : Window
    {
        DataClasses1DataContext db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        private Users_Table selectedTable;
        public HomeLibrarian()
        {
            InitializeComponent();
            cmbTable.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTable.SelectedIndex == 0)
            {
                var rows = from r in db.LibraryBooks select r;
                dgTable.ItemsSource = rows.ToList();
            }
            else if (cmbTable.SelectedIndex == 1)
            {
                var rows = from r in db.BookGenres select r;
                dgTable.ItemsSource = rows.ToList();
            }
            else if (cmbTable.SelectedIndex == 2)
            {
                var rows = from r in db.Students select r;
                dgTable.ItemsSource = rows.ToList();
            }
        }
        private void deleteSelectedRowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
