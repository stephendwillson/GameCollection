using GameCollection.Windows;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace GameCollection.Windows {

    //TODO: Fix odd behavior on "Done editing" console family combobox
    public partial class Window_GameCollectionMain : Window {

        public Window_GameCollectionMain() {

            InitializeComponent();

            //Load up combo box for console family names from database
            try {
                string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                
                command.CommandText = "select FamilyName from ConsoleFamily";
                SqlDataReader reader = command.ExecuteReader();

                ComboBox_ConsoleFamilyName.Items.Clear();
                while (reader.Read()) {
                    ComboBox_ConsoleFamilyName.Items.Add(reader[0].ToString());
                }
                if (ComboBox_ConsoleFamilyName.Items.Count != 0 && ComboBox_ConsoleFamilyName.SelectedItem == null) {
                    ComboBox_ConsoleFamilyName.SelectedItem = ComboBox_ConsoleFamilyName.Items[0];
                }
                reader.Close();

                command.Dispose();
                connection.Close();

                UpdateConsoleNameComboBox();

            }
            catch (Exception ex) {
                MessageBox.Show("Failed to load main window: " + ex);
                Application.Current.Shutdown();
            }
        }

        private void Button_AddAConsole_Click(object sender, RoutedEventArgs e) {

            Window_AddAConsole addConsole = new Window_AddAConsole();
            addConsole.Show();
            this.Close();
        }

        private void Button_AddAConsoleFamily_Click(object sender, RoutedEventArgs e) {

            Window_AddAConsoleFamily addConsoleFamily = new Window_AddAConsoleFamily();
            addConsoleFamily.Show();
            this.Close();
        }

        private void ComboBox_ConsoleFamilyName_DropDownClosed(object sender, EventArgs e) {

            UpdateConsoleNameComboBox();
        }

        private void UpdateConsoleNameComboBox() {

            string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();

            command.CommandText = "select Name " +
                "from Console " +
                "where ConsoleFamilyID = (select ID from ConsoleFamily where FamilyName = '" + ComboBox_ConsoleFamilyName.Text + "')";
            SqlDataReader reader = command.ExecuteReader();

            ComboBox_ConsoleName.Items.Clear();
            while (reader.Read()) {
                ComboBox_ConsoleName.Items.Add(reader[0].ToString());
            }
            if (ComboBox_ConsoleName.Items.Count != 0 && ComboBox_ConsoleName.SelectedItem == null) {
                ComboBox_ConsoleName.SelectedItem = ComboBox_ConsoleName.Items[0];
            }
            reader.Close();
            command.Dispose();

            connection.Close();
        }
    }
}