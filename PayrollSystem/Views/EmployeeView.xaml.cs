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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PayrollSystem.SubWindow;
using MySql.Data.MySqlClient;
using PayrollSystem.ListItem;

namespace PayrollSystem.Views
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        public string connection;

        public EmployeeView()
        {
            InitializeComponent();
        }

        private void AddNewEmployee(object sender, RoutedEventArgs e){
            AddEmployee employee = new AddEmployee(this.connection,this);
            employee.Show();
            return;
        }

        public void ReloadListView(){
            ToDisplay.Children.Clear();
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string command = "SELECT EMPLOYEE_ID, CONCAT(EMPLOYEE_FIRST_NAME, ' ', EMPLOYEE_LAST_NAME), EMPLOYEE_POSITION FROM user_employee;";
            string[] information = new string[4];

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    information[0] = rdr[0].ToString();
                    information[1] = rdr[1].ToString();
                    information[2] = rdr[2].ToString();
                    information[3] = GetPositionRate(rdr[2].ToString());

                    EmployeeItem item = new EmployeeItem(information, this.connection, this);
                    ToDisplay.Children.Add(item);

                }
                rdr.Close();

            } catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            finally
            {
                conn.Close();
            }
            return;
        }
        private string GetPositionRate(string position)
        {
            string rate = "";

            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;
            string command = $"SELECT POSITION_RATE_PER_HOUR FROM user_position WHERE POSITION_TITLE = '{position}';";
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();
                rdr.Read();
                rate = rdr[0].ToString();
                rdr.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            finally
            {
                conn.Close();
            }
            return rate;
        }
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                ToDisplay.Children.Clear();
                MySqlConnection conn = new MySqlConnection(this.connection);
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataReader rdr;

                string command = $"SELECT EMPLOYEE_ID, CONCAT(EMPLOYEE_FIRST_NAME, ' ', EMPLOYEE_LAST_NAME), EMPLOYEE_POSITION FROM user_employee WHERE EMPLOYEE_FIRST_NAME LIKE '{SearchBox.Text}%';";
                string[] information = new string[4];

                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        information[0] = rdr[0].ToString();
                        information[1] = rdr[1].ToString();
                        information[2] = rdr[2].ToString();
                        information[3] = GetPositionRate(rdr[2].ToString());

                        EmployeeItem item = new EmployeeItem(information, this.connection, this);
                        ToDisplay.Children.Add(item);
                    }
                    rdr.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                ReloadListView();
            }
            return;
        }
    }
}
