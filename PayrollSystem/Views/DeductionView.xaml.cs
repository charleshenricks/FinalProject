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
    /// Interaction logic for DeductionView.xaml
    /// </summary>
    public partial class DeductionView : UserControl
    {
        public string connection;
        public DeductionView()
        {
            InitializeComponent();
        }

        private void AddDeduction(object sender, RoutedEventArgs e)
        {
            AddDeduction deduction = new AddDeduction(this.connection, this);
            deduction.Show();

            return;
        }

        public void ReloadListView()
        {
            ToDisplay.Children.Clear();
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string command = "SELECT * FROM user_deduction;";
            try
            {
                conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string[] information = { rdr[0].ToString(), rdr[1].ToString() };
                    DeductionItem item = new DeductionItem(information, this.connection, this);
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
            return;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                ToDisplay.Children.Clear();
                MySqlConnection conn = new MySqlConnection(this.connection);
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataReader rdr;

                string command = $"SELECT * FROM user_deduction WHERE DEDUCTION_DESCRIPTION LIKE '{SearchBox.Text}%';";
                try
                {
                    conn.Open();

                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string[] information = { rdr[0].ToString(), rdr[1].ToString() };
                        DeductionItem item = new DeductionItem(information, this.connection, this);
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
