using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class HomeLibrarian : Window
    {
        DataClasses1DataContext db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        private TextBox[] addtbxs,updatetbxs;
        private int cols = 0;
        public HomeLibrarian()
        {
            InitializeComponent();
            cmbTable.SelectedIndex = 0;
        }
        private void ReloadTable()
        {
            if (cmbTable.SelectedIndex == 0)
            {
                dgTable.ItemsSource = db.LibraryBooks.ToList();
            }
            else if (cmbTable.SelectedIndex == 1)
            {
                dgTable.ItemsSource = db.BookGenres.ToList();
            }
            else if (cmbTable.SelectedIndex == 2)
            {
                dgTable.ItemsSource = db.Students.ToList();
            }
            else if (cmbTable.SelectedIndex == 3)
            {
                dgTable.ItemsSource = db.Courses.ToList();
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
                    if (cmbTable.SelectedIndex == 0 && dgTable.SelectedItem is LibraryBook sItem0)
                    {
                        var row = (from r in db.LibraryBooks
                                   where r.Book_ID == sItem0.Book_ID
                                   select r).SingleOrDefault();
                        row.Book_Title = input[1];
                        row.Book_Author = input[2];
                        row.Book_ISBN = input[3];
                        row.Book_PublicationYear = int.Parse(input[4]);
                        row.BookGenre_ID = int.Parse(input[5]);
                        row.Available_Quantity = int.Parse(input[6]);
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                        }
                    }
                    else if (cmbTable.SelectedIndex == 1 && dgTable.SelectedItem is BookGenre sItem1)
                    {
                        var row = (from r in db.BookGenres
                                   where r.BookGenre_ID == sItem1.BookGenre_ID
                                   select r).SingleOrDefault();
                        row.BookGenre_ID = int.Parse(input[0]);
                        row.BookGenre_Desc = input[1];
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                        }
                    }
                    else if (cmbTable.SelectedIndex == 2 && dgTable.SelectedItem is Student sItem2)
                    {
                        var row = (from r in db.Students
                                   where r.Student_ID == sItem2.Student_ID
                                   select r).SingleOrDefault();
                        row.Student_Name = input[1];
                        row.Student_Email = input[2];
                        row.Student_ContactNum = input[3];
                        row.Student_Address = input[4];
                        row.Course_ID = int.Parse(input[5]);
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
                        }
                    }
                    else if (cmbTable.SelectedIndex == 3 && dgTable.SelectedItem is Course sItem3)
                    {
                        var row = (from r in db.Courses
                                   where r.Course_ID == sItem3.Course_ID
                                   select r).SingleOrDefault();
                        row.Course_ID = int.Parse(input[0]);
                        row.Course_Name = input[1];
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating row: " + ex.Message);
                            lbChangedStatus.Content = "Error updating row.";
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

                input[x] = addtbxs[x].Text.Trim();

                if(string.IsNullOrEmpty(input[x]))
                    isNullorEmpty = true;
            }
            
            if (!isNullorEmpty)
            {
                if (cmbTable.SelectedIndex == 0)
                {
                    if (!db.LibraryBooks.Any(u => u.Book_ID == int.Parse(input[0])))
                    {
                        var newBook = new LibraryBook
                        {
                            Book_ID = int.Parse(input[0]),
                            Book_Title = input[1],
                            Book_Author = input[2],
                            Book_ISBN = input[3],
                            Book_PublicationYear = int.Parse(input[4]),
                            BookGenre_ID = int.Parse(input[5]),
                            Available_Quantity = int.Parse(input[6])
                        };
                        db.LibraryBooks.InsertOnSubmit(newBook);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Book added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding book: " + ex.Message);
                            lbChangedStatus.Content = "Error adding book.";
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Book with ID '{int.Parse(input[0])}' already exists.");
                        lbChangedStatus.Content = "Book ID already exists.";
                    }
                }
                else if (cmbTable.SelectedIndex == 1)
                {
                    if (!db.BookGenres.Any(u => u.BookGenre_ID == int.Parse(input[0])))
                    {
                        var newBookGenre = new BookGenre
                        {
                            BookGenre_ID = int.Parse(input[0]),
                            BookGenre_Desc = input[1],
                        };
                        db.BookGenres.InsertOnSubmit(newBookGenre);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Genre added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding genre: " + ex.Message);
                            lbChangedStatus.Content = "Error adding genre.";
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Genre with ID '{int.Parse(input[0])}' already exists.");
                        lbChangedStatus.Content = "Genre ID already exists.";
                    }
                }
                else if (cmbTable.SelectedIndex == 2)
                {
                    if (!db.Students.Any(u => u.Student_ID == input[0]))
                    {
                        var newStudent = new Student
                        {
                            Student_ID = input[0],
                            Student_Name = input[1],
                            Student_Email = input[2],
                            Student_ContactNum = input[3],
                            Student_Address = input[4],
                            Course_ID = int.Parse(input[5])
                        };
                        db.Students.InsertOnSubmit(newStudent);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Student added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding student: " + ex.Message);
                            lbChangedStatus.Content = "Error adding student.";
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Student with ID '{input[0]}' already exists.");
                        lbChangedStatus.Content = "Student ID already exists.";
                    }
                }
                else if (cmbTable.SelectedIndex == 3)
                {
                    if (!db.Courses.Any(u => u.Course_ID == int.Parse(input[0])))
                    {
                        var newCourse = new Course
                        {
                            Course_ID = int.Parse(input[0]),
                            Course_Name = input[1],
                        };
                        db.Courses.InsertOnSubmit(newCourse);

                        try
                        {
                            db.SubmitChanges();

                            ReloadTable();

                            lbChangedStatus.Content = $"Course added successfully.";

                            for (int x = 0; x < cols; x++)
                            {
                                addtbxs[x].Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding course: " + ex.Message);
                            lbChangedStatus.Content = "Error adding course.";
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Course with ID '{int.Parse(input[0])}' already exists.");
                        lbChangedStatus.Content = "Course ID already exists.";
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

                        if(cmbTable.SelectedIndex == 0 && dgTable.SelectedItem is LibraryBook selectedItem0)
                            db.LibraryBooks.DeleteOnSubmit(selectedItem0);
                        else if (cmbTable.SelectedIndex == 1 && dgTable.SelectedItem is BookGenre selectedItem1)
                            db.BookGenres.DeleteOnSubmit(selectedItem1);
                        else if (cmbTable.SelectedIndex == 2 && dgTable.SelectedItem is Student selectedItem2)
                            db.Students.DeleteOnSubmit(selectedItem2);
                        else if (cmbTable.SelectedIndex == 3 && dgTable.SelectedItem is Course selectedItem3)
                            db.Courses.DeleteOnSubmit(selectedItem3);
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
            if (dgTable.SelectedItem != null)
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
            if (cmbTable.SelectedIndex == 0)
            {
                cols = 7;
                widths = new int[] { 30, 250, 125, 150, 50, 30, 40 };
                dgTable.ItemsSource = db.LibraryBooks.ToList();
            }
            else if (cmbTable.SelectedIndex == 1)
            {
                cols = 2;
                widths = new int[] { 30, 100 };
                dgTable.ItemsSource = db.BookGenres.ToList();
            }
            else if (cmbTable.SelectedIndex == 2)
            {
                cols = 6;
                widths = new int[] { 60, 150, 175, 100, 225, 50 };
                dgTable.ItemsSource = db.Students.ToList();
            }
            else if (cmbTable.SelectedIndex == 3)
            {
                cols = 2;
                widths = new int[] { 30, 300 };
                dgTable.ItemsSource = db.Courses.ToList();
            }

            addtbxs = new TextBox[cols];
            updatetbxs = new TextBox[cols];

            for (int x = 0; x < cols; x++)
            {
                addtbxs[x] = new TextBox() { Width = widths[x] };
                spAdd.Children.Add(addtbxs[x]);
                updatetbxs[x] = new TextBox() { Width = widths[x] };
                if (x == 0)
                    updatetbxs[x] = new TextBox() { Width = widths[x], IsEnabled = false };
                spUpdate.Children.Add(updatetbxs[x]);
            }
        }
    }
}
