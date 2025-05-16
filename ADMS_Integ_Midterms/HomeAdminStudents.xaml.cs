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
        private Users_Table selectedTable;
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
            string userId = addUserIdTextBox.Text.Trim();
            string password = addPasswordTextBox.Text.Trim();
            string userName = addUserNameTextBox.Text.Trim();
            string selectedRole = (addRoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();


            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(selectedRole))
            {
                if (!db.Users_Tables.Any(u => u.User_ID == userId))
                {
                    var newUser = new Users_Table
                    {
                        User_ID = userId,
                        User_Pass = password,
                        Username = userName,
                        Access_Level = selectedRole
                    };
                    db.Users_Tables.InsertOnSubmit(newUser);

                    try
                    {
                        db.SubmitChanges();

                        LoadUsers();

                        statusTextBlock.Text = $"User '{userId}' added successfully with Role: '{selectedRole}'.";
                        addUserIdTextBox.Clear();
                        addPasswordTextBox.Clear();
                        addUserNameTextBox.Clear();
                        addRoleComboBox.SelectedIndex = -1;
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
                MessageBox.Show("User ID, password, user name, and role cannot be empty.");
                statusTextBlock.Text = "User ID, password, user name, and role cannot be empty.";
            }
        }

        private void userDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userDataGrid.SelectedItem != null && userDataGrid.SelectedItem is Users_Table selectedUser)
            {
                selectedTable = selectedUser;

                updateUserIdTextBox.Text = selectedTable.User_ID;
                updatePasswordTextBox.Text = selectedTable.User_Pass;
                updateUserNameTextBox.Text = selectedTable.Username;
                updateRoleComboBox.SelectedItem = selectedTable.Access_Level;

                viewUserIdTextBox.Text = selectedTable.User_ID;
                viewPasswordTextBox.Text = selectedTable.User_Pass;
                viewUserNameTextBox.Text = selectedTable.Username;
                viewRoleTextBox.Text = selectedTable.Access_Level;

                saveUserButton.IsEnabled = true;
                deleteSelectedUserButton.IsEnabled = true;
            }
            else
            {
                selectedUser = null;
                updateUserIdTextBox.Clear();
                updatePasswordTextBox.Clear();
                updateUserNameTextBox.Clear();
                updateRoleComboBox.SelectedIndex = -1;
                viewUserIdTextBox.Clear();
                viewPasswordTextBox.Clear();
                viewUserNameTextBox.Clear();
                viewRoleTextBox.Clear();
                saveUserButton.IsEnabled = false;
                deleteSelectedUserButton.IsEnabled = false;
            }
        }

        private void saveUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTable != null)
            {
                selectedTable.User_Pass = updatePasswordTextBox.Text.Trim();
                selectedTable.Username = updateUserNameTextBox.Text.Trim();
                string selectedRole = (updateRoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (!string.IsNullOrEmpty(selectedRole))
                {
                    selectedTable.Access_Level = selectedRole;

                    try
                    {
                        db.SubmitChanges();
                        LoadUsers();

                        statusTextBlock.Text = $"User '{selectedTable.User_ID}' updated successfully with Role: '{selectedRole}'.";
                        updateUserIdTextBox.Clear();
                        updatePasswordTextBox.Clear();
                        updateUserNameTextBox.Clear();
                        updateRoleComboBox.SelectedIndex = -1;
                        viewUserIdTextBox.Clear();
                        viewPasswordTextBox.Clear();
                        viewUserNameTextBox.Clear();
                        viewRoleTextBox.Clear();
                        saveUserButton.IsEnabled = false;
                        selectedTable = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating user: " + ex.Message);
                        statusTextBlock.Text = "Error updating user.";
                    }
                }
                else
                {
                    MessageBox.Show("Please select a role to update.");
                    statusTextBlock.Text = "Please select a role to update.";
                }
            }
            else
            {
                MessageBox.Show("No user selected for update.");
                statusTextBlock.Text = "No user selected for update.";
            }
        }

        private void deleteSelectedUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (userDataGrid.SelectedItem != null && userDataGrid.SelectedItem is Users_Table selectedUser)
            {
                selectedTable = selectedUser;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete user '{selectedTable.User_ID}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        string deletedUserID = selectedTable.User_ID;

                        db.Users_Tables.DeleteOnSubmit(selectedTable);
                        db.SubmitChanges();
                        db = new DataClasses1DataContext(Properties.Settings.Default.Northville_LibraryConnectionString);
                        LoadUsers();

                        statusTextBlock.Text = $"User '{deletedUserID}' deleted successfully.";
                        deleteSelectedUserButton.IsEnabled = false;
                        selectedTable = null;
                        updateUserIdTextBox.Clear();
                        updatePasswordTextBox.Clear();
                        updateUserNameTextBox.Clear();
                        updateRoleComboBox.SelectedIndex = -1;
                        viewUserIdTextBox.Clear();
                        viewPasswordTextBox.Clear();
                        viewUserNameTextBox.Clear();
                        viewRoleTextBox.Clear();
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
