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
using MySql.Data.MySqlClient;
using PayrollSystem.ListItem;

namespace PayrollSystem.Views
{
    /// <summary>
    /// Interaction logic for PayrollView.xaml
    /// </summary>
    public partial class PayrollView : UserControl
    {
        public string connection;
        public PayrollView()
        {
            InitializeComponent();
        }

        public void ReloadListView()
        {

            // Employee - id, name, rate per hour // Position
            // Deduction - amount
            // Cash Advance - amount

            ToDisplay.Children.Clear();

            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;
            string[] information = new string[5];
            try
            {
                conn.Open();

                string command = "SELECT EMPLOYEE_ID, CONCAT(EMPLOYEE_FIRST_NAME, ' ', EMPLOYEE_LAST_NAME), EMPLOYEE_POSITION FROM user_employee;";

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    information[0] = rdr[0].ToString();
                    information[1] = rdr[1].ToString();
                    information[2] = GetPositionRate(rdr[2].ToString());
                    information[3] = Convert.ToString(GetDeduction());
                    string employee_id = rdr[0].ToString();
                    information[4] = GetCashAdvance(employee_id);
                    PayrollItem item = new PayrollItem( information,this);
                    ToDisplay.Children.Add(item);
                }
                rdr.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"Hakdog {err.Message}");
            }
            finally
            {
                conn.Close();
            }
            
            return;
        }

        private string GetPositionRate(string position_name)
        {
            string position_rate = "";
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;
            try
            {
                conn.Open();
                string command = $"SELECT POSITION_RATE_PER_HOUR FROM user_position WHERE POSITION_TITLE = '{position_name}';";

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();
                rdr.Read();
                position_rate = rdr[0].ToString();
                rdr.Close();

            }
            catch (Exception err)
            {
                Console.WriteLine($"1 {err.Message}");
            } 
            finally
            {
                conn.Close();
            }
            return position_rate;
        }

        private int GetDeduction()
        {
            int deduction = 0;
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;
            try
            {
                conn.Open();
                string command = "SELECT DEDUCTION_AMOUNT FROM user_deduction;";

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    
                    deduction += Convert.ToInt32(rdr[0]);
                }
                rdr.Close();

            }
            catch (Exception err)
            {
                Console.WriteLine($"2 {err.Message}");
            }
            finally
            {
                conn.Close();
            }
            return deduction;
        }

        private string GetCashAdvance(string employee_id)
        {
            string cash_advance = "0";
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;
            try
            {
                conn.Open();
                string command = $"SELECT CASH_ADVANCE_AMOUNT FROM user_advance WHERE CASH_ADVANCE_EMPLOYEE_ID = '{employee_id}';";

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!string.IsNullOrWhiteSpace(rdr[0].ToString()))
                    {
                        cash_advance = rdr[0].ToString();
                    }
                }
                rdr.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"3 {err.Message}");
            }
            finally
            {
                conn.Close();
            }
            return cash_advance;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                ToDisplay.Children.Clear();

                MySqlConnection conn = new MySqlConnection(this.connection);
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataReader rdr;
                string[] information = new string[5];
                try
                {
                    conn.Open();

                    string command = $"SELECT EMPLOYEE_ID, CONCAT(EMPLOYEE_FIRST_NAME, ' ', EMPLOYEE_LAST_NAME), EMPLOYEE_POSITION FROM user_employee WHERE EMPLOYEE_FIRST_NAME LIKE '{SearchBox.Text}%';";

                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        information[0] = rdr[0].ToString();
                        information[1] = rdr[1].ToString();
                        information[2] = GetPositionRate(rdr[2].ToString());
                        information[3] = Convert.ToString(GetDeduction());
                        string employee_id = rdr[0].ToString();
                        information[4] = GetCashAdvance(employee_id);
                        PayrollItem item = new PayrollItem(information, this);
                        ToDisplay.Children.Add(item);
                    }
                    rdr.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine($"Hakdog {err.Message}");
                }
                finally
                {
                    conn.Close();
                }
                return;
            } else
            {
                ReloadListView();
            }
        }
    }
}
