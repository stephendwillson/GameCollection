using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Configuration;

namespace GameCollection.Windows {
    /// <summary>
    /// Interaction logic for Window_AddAConsole.xaml
    /// </summary>
    public partial class Window_AddAConsole : Window {

        public Window_AddAConsole() {

            InitializeComponent();

            try {

                string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                //Load combo box for condition names
                command.CommandText = "select Name from Condition";
                SqlDataReader reader = command.ExecuteReader();

                ComboBox_ConsoleCondition.Items.Clear();
                while (reader.Read()) {
                    ComboBox_ConsoleCondition.Items.Add(reader[0].ToString());
                }
                if (ComboBox_ConsoleCondition.Items.Count != 0 && ComboBox_ConsoleCondition.SelectedItem == null) {
                    ComboBox_ConsoleCondition.SelectedItem = ComboBox_ConsoleCondition.Items[0].ToString();
                }
                reader.Close();

                //Load combo box for console family names
                command.CommandText = "select FamilyName from ConsoleFamily";
                reader = command.ExecuteReader();

                ComboBox_ConsoleFamily.Items.Clear();
                while (reader.Read()) {
                    ComboBox_ConsoleFamily.Items.Add(reader[0].ToString());
                }
                if (ComboBox_ConsoleFamily.Items.Count != 0 || ComboBox_ConsoleFamily.SelectedItem == null) {
                    ComboBox_ConsoleFamily.SelectedItem = ComboBox_ConsoleFamily.Items[0].ToString();
                }
                reader.Close();

                command.Dispose();
                connection.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("Failed to load 'Add a Console' window: " + ex);
            }
        }

        private void Button_AddAConsole_Click(object sender, RoutedEventArgs e) {

            try {

                string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand command = connection.CreateCommand(); ;
                
                command.CommandText = "select id from Condition where Name = '" + ComboBox_ConsoleCondition.Text + "'";
                object objectConditionID = command.ExecuteScalar();

                command.CommandText = "select id from ConsoleFamily where FamilyName = '" + ComboBox_ConsoleFamily.Text + "'";
                object objectFamilyID = command.ExecuteScalar();

                string name = TextBox_ConsoleName.Text;
                string manufacturer = TextBox_ConsoleManufacturer.Text;
                int releaseYear = Convert.ToInt32(TextBox_ConsoleReleaseYear.Text);
                int conditionID = Convert.ToInt32(objectConditionID);
                int consoleFamilyID = Convert.ToInt32(objectFamilyID);
                string notes = TextBox_ConsoleNotes.Text;

                command.CommandText = "insert into Console " +
                    "(Name,Manufacturer,ReleaseYear,ConditionID,ConsoleFamilyID,Notes) " +
                    " values ('" + name + "', '" + manufacturer + "', '" + releaseYear + "', '" + conditionID + "', '" + consoleFamilyID + "', '" + notes + "');";

                command.ExecuteNonQuery();
                
                command.Dispose();
                connection.Close();

                Window_GameCollectionMain gameCollectionMain = new Window_GameCollectionMain();
                gameCollectionMain.Show();
                this.Close();

            }
            catch (Exception ex) {
                MessageBox.Show("Error adding console to database: " + ex);
            }
        }

        private void TextBox_ReleaseYear_PreviewTextInput(object sender, TextCompositionEventArgs e) {

            //Force only digits in Release Year text box
            if (!char.IsDigit(e.Text, e.Text.Length - 1)) {
                e.Handled = true;

            }
        }
    }
}
