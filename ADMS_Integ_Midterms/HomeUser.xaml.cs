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
    /// Interaction logic for HomeUser.xaml
    /// </summary>
    public partial class HomeUser : Window
    {
        public HomeUser()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to log out?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;

            }
        }
    }
}
