using System.Data.SqlClient;
using System.Windows;
using System.Configuration;

namespace GameCollection.Windows {
    /// <summary>
    /// Interaction logic for Window_AddAConsoleFamily.xaml
    /// </summary>
    public partial class Window_AddAConsoleFamily : Window {

        public Window_AddAConsoleFamily() {

            InitializeComponent();
        }

        private void Button_AddConsoleFamily_Click(object sender, RoutedEventArgs e) {

            //Insert entered value into ConsoleFamily table
            string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "insert into ConsoleFamily " +
                "(FamilyName) " +
                " values ('" + TextBox_ConsoleFamilyName.Text + "');";
            command.ExecuteNonQuery();

            connection.Close();

            //Reopen main window
            Window_GameCollectionMain gameCollectionMain = new Window_GameCollectionMain();
            gameCollectionMain.Show();
            this.Close();
        }
    }
}
