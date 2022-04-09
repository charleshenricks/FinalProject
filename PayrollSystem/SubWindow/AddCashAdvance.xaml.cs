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
    /// Interaction logic for AddCashAdvance.xaml
    /// </summary>
    public partial class AddCashAdvance : Window
    {
        private string connection;
        private CashAdvanceView main;

        public AddCashAdvance(string connection, CashAdvanceView main)
        {
            InitializeComponent();
            this.main = main;
            this.connection = connection;
            AddName(connection);
        }

        public void AddName(string connection)
        {
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string command = "SELECT CONCAT(EMPLOYEE_FIRST_NAME, ' ',EMPLOYEE_LAST_NAME) FROM user_employee;";

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

                    CashAdvanceName.Items.Add(item);
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

            return;
        }

        private string GetEmployeeID()
        {
            string EmployeeID = "";
            
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string name = "";
            foreach (char n in CashAdvanceName.Text)
            {
                if (char.IsWhiteSpace(n))
                {
                    break;
                } else
                {
                    name += $"{n}";
                }
            }

            try
            {
                conn.Open();
                string command = $"SELECT EMPLOYEE_ID FROM user_employee WHERE EMPLOYEE_FIRST_NAME LIKE '{name}%';";

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    EmployeeID = rdr[0].ToString();
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
            
            return EmployeeID;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();

            if (!(string.IsNullOrWhiteSpace(CashAdvanceName.Text)) &&
                !(string.IsNullOrWhiteSpace(CashAdvanceDate.Text)) &&
                !(string.IsNullOrWhiteSpace(CashAdvanceAmount.Text)))
            {
                string command = $"INSERT INTO user_advance VALUES ('{GetEmployeeID()}', '{CashAdvanceDate.Text}', '{CashAdvanceName.Text}', '{CashAdvanceAmount.Text}');";
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
            }
            else
            {
                TextBox[] box = { CashAdvanceDate, CashAdvanceAmount };
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

        private void CashAdvanceDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (CashAdvanceDate.Text == "Invalid Input")
            {
                CashAdvanceDate.Text = "";
                CashAdvanceDate.Foreground = Brushes.Black;
            }
            return;
        }

        private void CashAdvanceAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (CashAdvanceAmount.Text == "Invalid Input")
            {
                CashAdvanceAmount.Text = "";
                CashAdvanceAmount.Foreground = Brushes.Black;
            }
            return;
        }
    }
}
