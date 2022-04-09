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
    /// Interaction logic for PositionView.xaml
    /// </summary>
    public partial class PositionView : UserControl
    {
        public string connection;

        public PositionView()
        {
            InitializeComponent();
        }

        private void AddPosition(object sender, RoutedEventArgs e){
            AddPosition position = new AddPosition(this.connection, this);
            position.Show();
            return;
        }


        public void ReloadListView(){
            ToDisplay.Children.Clear();
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string command = "SELECT * FROM user_position;";
            try
            {
                conn.Open();
                
                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string[] information = { rdr[0].ToString(), rdr[1].ToString() };
                    PositionItem item = new PositionItem(information, this.connection, this);
                    ToDisplay.Children.Add(item);
                }
                rdr.Close();

            } catch (Exception err)
            {
                Console.WriteLine(err.Message);
            } finally
            {
                conn.Close();
            }
            return;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {

            ToDisplay.Children.Clear();
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr;

            string command = $"SELECT * FROM user_position WHERE POSITION_TITLE LIKE '{SearchBox.Text}%';";
            try
            {
                conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = command;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string[] information = { rdr[0].ToString(), rdr[1].ToString() };
                    PositionItem item = new PositionItem(information, this.connection, this);
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


    }
}
