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

namespace PayrollSystem.SubWindow2
{
    /// <summary>
    /// Interaction logic for UpdateEmployee.xaml
    /// </summary>
    public partial class UpdateEmployee : Window
    {
        private string connection;
        private string old_item;
        private EmployeeView main;
        public UpdateEmployee(string information, string connection, EmployeeView main)
        {
            InitializeComponent();
            this.connection = connection;
            this.main = main;

            string name = "";
            foreach (char n in information)
            {
                if (char.IsLetter(n))
                {
                    name += $"{n}";
                } else
                {
                    break;
                }
            }
            SetValue(name);
            AddPosition(connection);
            this.old_item = EmployeeFirstName.Text;
            return;
        } 

        private void SetValue(string name)
        {
            string[] information = new string[7];
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            try
            {
                conn.Open();
                string command = $"SELECT EMPLOYEE_FIRST_NAME, EMPLOYEE_LAST_NAME, EMPLOYEE_ADDRESS, EMPLOYEE_BIRTHDATE, EMPLOYEE_CONTACT, EMPLOYEE_GENDER, EMPLOYEE_POSITION FROM user_employee WHERE EMPLOYEE_FIRST_NAME LIKE '{name}%';";

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    for (int i=0; i<7; i++)
                    {
                        information[i] = rdr[i].ToString();
                    }
                }
                rdr.Close();

                TextBox[] box = { EmployeeFirstName, EmployeeLastName, EmployeeAddress, EmployeeBirthDate, EmployeeContact };
                for (int i=0; i<5; i++)
                {
                    box[i].Text = information[i];
                }
                EmployeeGender.Text = information[5];
                EmployeePosition.Text = information[6];
            } catch (Exception err)
            {
                MessageBox.Show(err.Message);
            } finally
            {
                conn.Close();
            }
            return;
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

                    EmployeePosition.Items.Add(item);
                }
                rdr.Close();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
                conn.Close();
            }
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

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
          
            if (!string.IsNullOrWhiteSpace(EmployeeFirstName.Text) &&
                !string.IsNullOrWhiteSpace(EmployeeLastName.Text) &&
                !string.IsNullOrWhiteSpace(EmployeeAddress.Text) &&
                !string.IsNullOrWhiteSpace(EmployeeBirthDate.Text) &&
                !string.IsNullOrWhiteSpace(EmployeeContact.Text) &&
                !string.IsNullOrWhiteSpace(EmployeeGender.Text) &&
                !string.IsNullOrWhiteSpace(EmployeePosition.Text))
            {
                string command = $"UPDATE user_employee SET " +
                                 $"EMPLOYEE_FIRST_NAME = '{EmployeeFirstName.Text}', " +
                                 $"EMPLOYEE_LAST_NAME = '{EmployeeLastName.Text}', " +
                                 $"EMPLOYEE_ADDRESS = '{EmployeeAddress.Text}', " +
                                 $"EMPLOYEE_BIRTHDATE = '{EmployeeBirthDate.Text}', " +
                                 $"EMPLOYEE_CONTACT = '{EmployeeContact.Text}', " +
                                 $"EMPLOYEE_GENDER = '{EmployeeGender.Text}', " +
                                 $"EMPLOYEE_POSITION = '{EmployeePosition.Text}' " +
                                 $"WHERE EMPLOYEE_FIRST_NAME = '{this.old_item}';";
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
                TextBox[] box = { EmployeeFirstName, EmployeeLastName, EmployeeAddress, EmployeeBirthDate, EmployeeContact };
                for ( int i=0; i < box.Length; i++)
                {
                    box[i].Text = "Invalid Input";
                    box[i].Foreground = Brushes.Red;
                }
            }

            this.main.ReloadListView();
            this.Close();
            return;
        }

        private void EmployeeFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (EmployeeFirstName.Text == "Invalid Input")
            {
                EmployeeFirstName.Text = "";
                EmployeeFirstName.Foreground = Brushes.Black;
            }
            return;
        }

        private void EmployeeLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (EmployeeLastName.Text == "Invalid Input")
            {
                EmployeeLastName.Text = "";
                EmployeeLastName.Foreground = Brushes.Black;
            }
            return;
        }

        private void EmployeeAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (EmployeeAddress.Text == "Invalid Input")
            {
                EmployeeAddress.Text = "";
                EmployeeAddress.Foreground = Brushes.Black;
            }
            return;
        }

        private void EmployeeBirthDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (EmployeeBirthDate.Text == "Invalid Input")
            {
                EmployeeBirthDate.Text = "";
                EmployeeBirthDate.Foreground = Brushes.Black;
            }
            return;
        }

        private void EmployeeContact_KeyDown(object sender, KeyEventArgs e)
        {
            if (EmployeeContact.Text == "Invalid Input")
            {
                EmployeeContact.Text = "";
                EmployeeContact.Foreground = Brushes.Black;
            }
            return;
        }
    }
}
