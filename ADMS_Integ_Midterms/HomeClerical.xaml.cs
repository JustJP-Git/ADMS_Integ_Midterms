using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
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
    public partial class HomeClerical : Window
    {
        DataClasses1DataContext db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        private TextBox[] addtbxs, updatetbxs;
        private DatePicker[] addcalendar = new DatePicker[3], updatecalendar = new DatePicker[3];
        private int cols = 0;
        public HomeClerical()
        {
            InitializeComponent();
            cmbTable.SelectedIndex = 0;
            //foreach(DatePicker cy in addcalendar)
            //{
            //    int index = Array.IndexOf(addcalendar, cy);
            //    if (index != -1)
            //    {
            //        addcalendar[index] = new DatePicker();
            //    }
            //}
            //foreach (DatePicker d in updatecalendar)
            //{
            //    int index = Array.IndexOf(updatecalendar, d);
            //    if (index != -1)
            //    {
            //        addcalendar[index] = new DatePicker();
            //    }
            //}
        }
        private void ReloadTable()
        {
            if (cmbTable.SelectedIndex == 0)
            {
                dgTable.ItemsSource = db.BorrowTransactions.ToList();
            }
            else if (cmbTable.SelectedIndex == 1)
            {
                dgTable.ItemsSource = db.BookReturnStatus.ToList();
            }
            else if (cmbTable.SelectedIndex == 2)
            {
                dgTable.ItemsSource = db.CollectedFines.ToList();
            }
            else if (cmbTable.SelectedIndex == 3)
            {
                dgTable.ItemsSource = db.LibraryVisits.ToList();
            }
            else if (cmbTable.SelectedIndex == 4)
            {
                dgTable.ItemsSource = db.LibraryBooks.ToList();
            }
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((MessageBox.Show("Do you want to log out?", "Confirm", MessageBoxButton.YesNo)) == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void UpdateRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTable.SelectedItem != null)
            {
                string[] input = new string[0];
                bool isNullorEmpty = false;
                for (int x = 0; x < cols; x++)
                {
                    string[] hold;
                    hold = input;
                    input = new string[x + 1];
                    for (int y = 0; y < hold.Count(); y++)
                        input[y] = hold[y];
                    input[x] = updatetbxs[x].Text.Trim();
                    if (string.IsNullOrEmpty(input[x]))
                        isNullorEmpty = true;
                }
                if (!isNullorEmpty)
                {
                    if (cmbTable.SelectedIndex == 0 && dgTable.SelectedItem is BorrowTransaction sItem0)
                    {
                        var row = (from r in db.BorrowTransactions
                                   where r.Borrow_ID == sItem0.Borrow_ID
                                   select r).SingleOrDefault();
                        row.Student = db.Students.Single(x => x.Student_ID == input[1]);
                        row.LibraryBook = db.LibraryBooks.Single(x => x.Book_ID == int.Parse(input[2]));
                        row.Borrow_Date = DateTime.Parse(updatecalendar[0].Text);
                        row.Due_Date = DateTime.Parse(updatecalendar[1].Text);
                        row.BookReturnStatus = db.BookReturnStatus.Single(x => x.ReturnStatus_ID == int.Parse(input[5]));
                        try
                        {
                            db.SubmitChanges();

                            updatecalendar[0].SelectedDate = null;
                            updatecalendar[1].SelectedDate = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                            return;
                        }
                    }
                    else if (cmbTable.SelectedIndex == 1 && dgTable.SelectedItem is BookReturnStatus sItem1)
                    {
                        var row = (from r in db.BookReturnStatus
                                   where r.ReturnStatus_ID == sItem1.ReturnStatus_ID
                                   select r).SingleOrDefault();
                        row.ReturnStatus_Desc = input[1];
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                            return;
                        }
                    }
                    else if (cmbTable.SelectedIndex == 2 && dgTable.SelectedItem is CollectedFine sItem2)
                    {
                        var row = (from r in db.CollectedFines
                                   where r.CollectedFines_ID == sItem2.CollectedFines_ID
                                   select r).SingleOrDefault();
                        row.BorrowTransaction = db.BorrowTransactions.Single(x => x.Borrow_ID == int.Parse(input[1]));
                        row.Days_Late = int.Parse(input[2]);
                        row.Accrued_Fines = int.Parse(input[3]);
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                            return;
                        }
                    }
                    else if (cmbTable.SelectedIndex == 3 && dgTable.SelectedItem is LibraryVisit sItem3)
                    {
                        var row = (from r in db.LibraryVisits
                                   where r.Visit_ID == sItem3.Visit_ID
                                   select r).SingleOrDefault();
                        row.Student = db.Students.Single(x => x.Student_ID == input[1]);
                        row.Visit_Date = DateTime.Parse(updatecalendar[2].Text);
                        try
                        {
                            db.SubmitChanges();

                            updatecalendar[2].SelectedDate = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                            return;
                        }
                    }
                    ReloadTable();

                    lbChangedStatus.Content = $"Row {dgTable.SelectedIndex + 1} updated successfully.";

                    for (int x = 0; x < cols; x++)
                    {
                        updatetbxs[x].Clear();
                    }


                    dgTable.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Each field cannot be empty.");
                    lbChangedStatus.Content = "Each field cannot be empty.";
                }
            }
            else
            {
                MessageBox.Show("No row selected for update.");
                lbChangedStatus.Content = "No row selected for update.";
            }
        }
        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            string[] input = new string[0];
            bool isNullorEmpty = false;
            for (int x = 0; x < cols; x++)
            {
                string[] hold;
                hold = input;
                input = new string[x + 1];

                for (int y = 0; y < hold.Count(); y++)
                    input[y] = hold[y];

                if (!((cmbTable.SelectedIndex == 0 && (x == 3 || x == 4)) || (cmbTable.SelectedIndex == 3 && x == 2)))
                {
                    input[x] = addtbxs[x].Text.Trim();


                    if (string.IsNullOrEmpty(input[x]))
                        isNullorEmpty = true;
                }
            }

            if (!isNullorEmpty)
            {
                if (cmbTable.SelectedIndex == 0)
                {
                    if (!db.BorrowTransactions.Any(u => u.Borrow_ID == int.Parse(input[0])))
                    {
                        var newBorrow = new BorrowTransaction
                        {
                            Borrow_ID = int.Parse(input[0]),
                            Student_ID = input[1],
                            Book_ID = int.Parse(input[2]),
                            Borrow_Date = DateTime.Parse(addcalendar[0].Text),
                            Due_Date = DateTime.Parse(addcalendar[1].Text),
                            ReturnStatus_ID = int.Parse(input[5]),
                        };
                        db.BorrowTransactions.InsertOnSubmit(newBorrow);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Transaction added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                            addcalendar[0].SelectedDate = null;
                            addcalendar[1].SelectedDate = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding transaction: " + ex.Message);
                            lbChangedStatus.Content = "Error adding transaction.";
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Borrow Transaction with ID '{int.Parse(input[0])}' already exists.");
                        lbChangedStatus.Content = "Borrow Transaction ID already exists.";
                    }
                }
                else if (cmbTable.SelectedIndex == 1)
                {
                    if (!db.BookReturnStatus.Any(u => u.ReturnStatus_ID == int.Parse(input[0])))
                    {
                        var newStatus = new BookReturnStatus
                        {
                            ReturnStatus_ID = int.Parse(input[0]),
                            ReturnStatus_Desc = input[1],
                        };
                        db.BookReturnStatus.InsertOnSubmit(newStatus);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"New Status added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding new status: " + ex.Message);
                            lbChangedStatus.Content = "Error adding new status.";
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Status with ID '{int.Parse(input[0])}' already exists.");
                        lbChangedStatus.Content = "Status ID already exists.";
                    }
                }
                else if (cmbTable.SelectedIndex == 2)
                {
                    if (!db.CollectedFines.Any(u => u.CollectedFines_ID == int.Parse(input[0])))
                    {
                        var newFine = new CollectedFine
                        {
                            CollectedFines_ID = int.Parse(input[0]),
                            Borrow_ID = int.Parse(input[1]),
                            Days_Late = int.Parse(input[2]),
                            Accrued_Fines = int.Parse(input[3])
                        };
                        db.CollectedFines.InsertOnSubmit(newFine);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Fine added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding fine: " + ex.Message);
                            lbChangedStatus.Content = "Error adding fine.";
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Fine with ID '{input[0]}' already exists.");
                        lbChangedStatus.Content = "Fine ID already exists.";
                    }
                }
                else if (cmbTable.SelectedIndex == 3)
                {
                    if (!db.LibraryVisits.Any(u => u.Visit_ID == int.Parse(input[0])))
                    {
                        var newVisit = new LibraryVisit
                        {
                            Visit_ID = int.Parse(input[0]),
                            Visit_Date = DateTime.Parse(addcalendar[2].Text)
                        };
                        db.LibraryVisits.InsertOnSubmit(newVisit);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Visit added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                            addcalendar[2].SelectedDate = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding visit: " + ex.Message);
                            lbChangedStatus.Content = "Error adding visit.";
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Visit with ID '{int.Parse(input[0])}' already exists.");
                        lbChangedStatus.Content = "Visit ID already exists.";
                    }
                }
            }
            else
            {
                MessageBox.Show("Each field cannot be empty.");
                lbChangedStatus.Content = "Each field cannot be empty.";
            }
        }
        private void deleteSelectedRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTable.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete row {dgTable.SelectedIndex}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deletedRow = dgTable.SelectedIndex;

                        if (cmbTable.SelectedIndex == 0 && dgTable.SelectedItem is BorrowTransaction selectedItem0)
                            db.BorrowTransactions.DeleteOnSubmit(selectedItem0);
                        else if (cmbTable.SelectedIndex == 1 && dgTable.SelectedItem is BookReturnStatus selectedItem1)
                            db.BookReturnStatus.DeleteOnSubmit(selectedItem1);
                        else if (cmbTable.SelectedIndex == 2 && dgTable.SelectedItem is CollectedFine selectedItem2)
                            db.CollectedFines.DeleteOnSubmit(selectedItem2);
                        else if (cmbTable.SelectedIndex == 3 && dgTable.SelectedItem is LibraryVisit selectedItem3)
                            db.LibraryVisits.DeleteOnSubmit(selectedItem3);
                        db.SubmitChanges();
                        db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);

                        ReloadTable();

                        for (int x = 0; x < cols; x++)
                        {
                            updatetbxs[x].Clear();
                        }

                        lbChangedStatus.Content = $"Row {deletedRow} deleted successfully.";
                        deleteSelectedRowButton.IsEnabled = false;
                        dgTable.SelectedIndex = -1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting row: {ex.Message}", "Deletion Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        lbChangedStatus.Content = "Error deleting row.";
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("No row selected for deletion.");
                lbChangedStatus.Content = "No row selected for deletion.";
            }
        }
        private void dgTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTable.SelectedItem != null && cmbTable.SelectedIndex != 4)
            {
                //dynamic table to list convertion
                var selectedItem = dgTable.SelectedItem;

                var properties = selectedItem.GetType().GetProperties();

                var values = new List<object>();

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(selectedItem);
                    values.Add(value);
                }

                //display to update textboxes
                for (int x = 0; x < cols; x++)
                {
                    updatetbxs[x].Text = values[x].ToString();
                    if (cmbTable.SelectedIndex == 0 && (x == 3 || x == 4))
                    {
                        updatecalendar[x - 3].Text = values[x].ToString();
                    }
                    else if (cmbTable.SelectedIndex == 3 && x == 2)
                    {
                        updatecalendar[x].Text = values[x].ToString();
                    }
                }
                deleteSelectedRowButton.IsEnabled = true;
            }
            else
            {
                deleteSelectedRowButton.IsEnabled = false;
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int[] widths = new int[0];
            spAdd.Children.Clear();
            spUpdate.Children.Clear();
            AddRowButton.IsEnabled = true;
            UpdateRowButton.IsEnabled = true;
            if (cmbTable.SelectedIndex == 0)
            {
                cols = 6;
                widths = new int[] { 40, 70, 50, 150, 150, 30};
                dgTable.ItemsSource = db.BorrowTransactions.ToList();
            }
            else if (cmbTable.SelectedIndex == 1)
            {
                cols = 2;
                widths = new int[] { 40, 110 };
                dgTable.ItemsSource = db.BookReturnStatus.ToList();
            }
            else if (cmbTable.SelectedIndex == 2)
            {
                cols = 4;
                widths = new int[] { 40, 40, 50, 80 };
                dgTable.ItemsSource = db.CollectedFines.ToList();
            }
            else if (cmbTable.SelectedIndex == 3)
            {
                cols = 3;
                widths = new int[] { 40, 70, 150 };
                dgTable.ItemsSource = db.LibraryVisits.ToList();
            }
            else if (cmbTable.SelectedIndex == 3)
            {
                cols = 3;
                widths = new int[] { 40, 70, 150 };
                dgTable.ItemsSource = db.LibraryVisits.ToList();
            }
            else if (cmbTable.SelectedIndex == 4)
            {
                cols = 7;
                widths = new int[] { 30, 250, 125, 150, 50, 30, 40 };
                dgTable.ItemsSource = db.LibraryBooks.ToList();
                AddRowButton.IsEnabled = false;
                UpdateRowButton.IsEnabled = false;
            }
            bool[][] numericsonly = new bool[][]
            {
                new bool[] { true, false, true, false, false, true },
                new bool[] { true, false },
                new bool[] { true, true, true, true },
                new bool[] { true, false },
                new bool[] { false, false, false, false, false ,false, false}
            };
            addtbxs = new TextBox[cols];
            updatetbxs = new TextBox[cols];

            for (int x = 0; x < cols; x++)
            {
                addtbxs[x] = new TextBox() { Width = widths[x] };
                updatetbxs[x] = new TextBox() { Width = widths[x] };

                if (cmbTable.SelectedIndex == 0 && (x == 3 || x == 4))
                {
                    addcalendar[x-3] = new DatePicker() { Width = widths[x] };
                    updatecalendar[x-3] = new DatePicker() { Width = widths[x] };
                    spAdd.Children.Add(addcalendar[x-3]);
                    spUpdate.Children.Add(updatecalendar[x - 3]);
                    continue;
                }
                if (cmbTable.SelectedIndex == 3 && x == 2)
                {
                    addcalendar[x] = new DatePicker() { Width = widths[x] };
                    updatecalendar[x] = new DatePicker() { Width = widths[x] };
                    spAdd.Children.Add(addcalendar[x]);
                    spUpdate.Children.Add(updatecalendar[x]);
                    continue;
                }

                
                if (numericsonly[cmbTable.SelectedIndex][x])
                {
                    addtbxs[x].PreviewKeyDown += NumericOnly_PreviewKeyDown;
                    updatetbxs[x].PreviewKeyDown += NumericOnly_PreviewKeyDown;
                    addtbxs[x].MaxLength = 10;
                    updatetbxs[x].MaxLength = 10;
                }
                if (cmbTable.SelectedIndex == 4)
                    addtbxs[x] = new TextBox() { Width = widths[x], IsEnabled = false };
                spAdd.Children.Add(addtbxs[x]);
                if (x == 0 || cmbTable.SelectedIndex == 4)
                    updatetbxs[x] = new TextBox() { Width = widths[x], IsEnabled = false };
                spUpdate.Children.Add(updatetbxs[x]);
            }

        }
        private void NumericOnly_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
                e.Key == Key.Enter || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 ||
                e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        //PreviewKeyDown="NumericOnly_PreviewKeyDown"
    }
}
