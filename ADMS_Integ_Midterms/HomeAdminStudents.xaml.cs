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
    /// Interaction logic for HomeAdminStudents.xaml
    /// </summary>
    public partial class HomeAdminStudents : Window
    {
        DataClasses1DataContext db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
        private Student selectedTable;
        public HomeAdminStudents()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var users = from u in db.Students select u;
                userDataGrid.ItemsSource = users.ToList();

                statusTextBlock.Text = "Users loaded successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
                statusTextBlock.Text = "Error loading users.";
            }
        }

        private void addNewUserButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = addID.Text.Trim();
            string name = addName.Text.Trim();
            string email = addEmail.Text.Trim();
            string contact = addContact.Text.Trim();
            string address = addAddress.Text.Trim();
            int.TryParse(addCourse.Text, out int course);


            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(addCourse.Text))
            {
                if (!db.Students.Any(u => u.Student_ID == userId))
                {
                    var newUser = new Student
                    {
                        Student_ID = userId,
                        Student_Name = name,
                        Student_Email = email,
                        Student_ContactNum = contact,
                        Student_Address = address,
                        Course_ID = course
                    };

                    db.Students.InsertOnSubmit(newUser);

                    try
                    {
                        db.SubmitChanges();

                        LoadUsers();

                        statusTextBlock.Text = $"User '{userId}' added successfully.";
                        addID.Clear();
                        addName.Clear();
                        addEmail.Clear();
                        addContact.Clear();
                        addAddress.Clear();
                        addCourse.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding user: " + ex.Message);
                        statusTextBlock.Text = "Error adding user.";
                    }
                }
                else
                {
                    MessageBox.Show($"User with ID '{userId}' already exists.");
                    statusTextBlock.Text = "User already exists.";
                }
            }
            else
            {
                MessageBox.Show("Ensure that no fields are left empty.");
                statusTextBlock.Text = "Ensure that no fields are left empty.";
            }
        }

        private void userDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userDataGrid.SelectedItem != null && userDataGrid.SelectedItem is Student selectedUser)
            {
                selectedTable = selectedUser;

                updateID.Text = selectedTable.Student_ID;
                updateName.Text = selectedTable.Student_Name;
                updateEmail.Text = selectedTable.Student_Email;
                updateContact.Text = selectedTable.Student_ContactNum;
                updateAddress.Text = selectedTable.Student_Address;
                updateCourse.Text = selectedTable.Course_ID.ToString();

                viewID.Text = selectedTable.Student_ID;
                viewName.Text = selectedTable.Student_Name;
                viewEmail.Text = selectedTable.Student_Email;
                viewContact.Text = selectedTable.Student_ContactNum;
                viewAddress.Text = selectedTable.Student_Address;
                viewCourse.Text = selectedTable.Course_ID.ToString();

                saveUserButton.IsEnabled = true;
                deleteSelectedUserButton.IsEnabled = true;
            }
            else
            {
                selectedUser = null;
                updateID.Clear();
                updateName.Clear();
                updateEmail.Clear();
                updateContact.Clear();
                updateAddress.Clear();
                updateCourse.Clear();

                viewID.Clear();
                viewName.Clear();
                viewEmail.Clear();
                viewContact.Clear();
                viewAddress.Clear();
                viewCourse.Clear();
                saveUserButton.IsEnabled = false;
                deleteSelectedUserButton.IsEnabled = false;
            }
        }

        private void saveUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTable != null)
            {
                selectedTable.Student_Name = updateName.Text.Trim();
                selectedTable.Student_Email = updateEmail.Text.Trim();
                selectedTable.Student_ContactNum = updateContact.Text.Trim();
                selectedTable.Student_Address = updateAddress.Text.Trim();
                selectedTable.Course = db.Courses.Single(x => x.Course_ID == int.Parse(updateCourse.Text.Trim()));

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating row: " + ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("No user selected for update.");
                statusTextBlock.Text = "No user selected for update.";
            }
        }

        private void updateCourse_TextChanged(object sender, TextChangedEventArgs e)
        {
            int courseId;
            if (int.TryParse(updateCourse.Text.Trim(), out courseId))
            {
                viewCourse.Text = courseId.ToString();
            }
        }

        private void deleteSelectedUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (userDataGrid.SelectedItem != null && userDataGrid.SelectedItem is Student selectedUser)
            {
                selectedTable = selectedUser;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete user '{selectedTable.Student_ID}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        string deletedUserID = selectedTable.Student_ID;

                        db.Students.DeleteOnSubmit(selectedTable);
                        db.SubmitChanges();
                        db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
                        LoadUsers();

                        statusTextBlock.Text = $"User '{deletedUserID}' deleted successfully.";
                        deleteSelectedUserButton.IsEnabled = false;
                        selectedTable = null;
                        updateID.Clear();
                        updateName.Clear();
                        updateEmail.Clear();
                        updateContact.Clear();
                        updateAddress.Clear();
                        updateCourse.Clear();

                        viewID.Clear();
                        viewName.Clear();
                        viewEmail.Clear();
                        viewContact.Clear();
                        viewAddress.Clear();
                        viewCourse.Clear();
                        saveUserButton.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}", "Deletion Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        statusTextBlock.Text = "Error deleting user.";

                    }
                }
            }
            else
            {
                MessageBox.Show("No user selected for deletion.");
                statusTextBlock.Text = "No user selected for deletion.";
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            HomeAdminPrompt admin = new HomeAdminPrompt();
            admin.Show();
            this.Close();
        }
    }
}