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
using MySql.Data.MySqlClient;
using PayrollSystem.Views;

namespace PayrollSystem.SubWindow
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        private string connection;
        private EmployeeView main;

        public AddEmployee(string connection, EmployeeView main)
        {
            InitializeComponent();
            this.connection = connection;
            this.main = main;
            AddPosition(connection);
            return;
        }

        private string GenerateEmployeeID()
        {
            string ID = "";

            char letters = '-';
            int numbers = 0;

            Random random = new Random();

            for (int i=0; i<4; i++)
            {
                letters = (char)random.Next(65, 91);
                ID += Convert.ToString(letters);
            }

            for (int i=0; i<4; i++)
            {
                numbers = random.Next(0, 10);
                ID += Convert.ToString(numbers);
            }
            return ID;
        }

        public void AddPosition(string connection)
        {
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string command = "SELECT POSITION_TITLE FROM user_position;";

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = rdr[0];
                    item.FontFamily = new FontFamily("Montserrat");
                    item.FontSize = 14;
                    item.Padding = new Thickness(5);

                    PositionBox.Items.Add(item);
                }
                rdr.Close();

            } 
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
                conn.Close();
            }

            return;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();

            if (!string.IsNullOrWhiteSpace(FirstNameBox.Text) &&
                !string.IsNullOrWhiteSpace(LastNameBox.Text) &&
                !string.IsNullOrWhiteSpace(AddressBox.Text) &&
                !string.IsNullOrWhiteSpace(BirthDateBox.Text) &&
                !string.IsNullOrWhiteSpace(ContactBox.Text) &&
                !string.IsNullOrWhiteSpace(GenderBox.Text) &&
                !string.IsNullOrWhiteSpace(PositionBox.Text))
            {
                string command = $"INSERT INTO user_employee VALUES (" +
                                 $"'{GenerateEmployeeID()}', '{FirstNameBox.Text}', '{LastNameBox.Text}'," +
                                 $"'{AddressBox.Text}', '{BirthDateBox.Text}', '{ContactBox.Text}', '{GenderBox.Text}'," +
                                 $"'{PositionBox.Text}');";
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                finally
                {
                    conn.Close();
                }
            } else
            {
                TextBox[] box = { FirstNameBox, LastNameBox, AddressBox, BirthDateBox, ContactBox };
                for (int i = 0; i < box.Length; i++)
                {
                    box[i].Text = "Invalid Input";
                    box[i].Foreground = Brushes.Red;
                }
            }
            
            this.main.ReloadListView();
            this.Close();
            return;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e){
            if (e.LeftButton == MouseButtonState.Pressed){
                this.DragMove();
            }
            return;
        }

        private void ExitWindow(object sender, RoutedEventArgs e){
            this.Close();
            return;
        }

        private void FirstNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (FirstNameBox.Text == "Invalid Input")
            {
                FirstNameBox.Text = "";
                FirstNameBox.Foreground = Brushes.Black;
            }
        }

        private void LastNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (LastNameBox.Text == "Invalid Input")
            {
                LastNameBox.Text = "";
                LastNameBox.Foreground = Brushes.Black;
            }
        }

        private void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (AddressBox.Text == "Invalid Input")
            {
                AddressBox.Text = "";
                AddressBox.Foreground = Brushes.Black;
            }
        }

        private void BirthDateBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (BirthDateBox.Text == "Invalid Input")
            {
                BirthDateBox.Text = "";
                BirthDateBox.Foreground = Brushes.Black;
            }
        }

        private void ContactBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (ContactBox.Text == "Invalid Input")
            {
                ContactBox.Text = "";
                ContactBox.Foreground = Brushes.Black;
            }
        }
    }
}
