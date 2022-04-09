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
using PayrollSystem.Views;
using MySql.Data.MySqlClient;

namespace PayrollSystem.SubWindow
{
    /// <summary>
    /// Interaction logic for AddPosition.xaml
    /// </summary>
    public partial class AddPosition : Window
    {
        private string connection;
        private PositionView main;
        public AddPosition(string connection, PositionView main)
        {
            InitializeComponent();
            this.connection = connection;
            this.main = main;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        { 
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();

            if (!(string.IsNullOrWhiteSpace(PositionTitle.Text)) &&
                !(string.IsNullOrWhiteSpace(PositionRate.Text)))
            {
                string command = $"INSERT INTO user_position VALUES ('{PositionTitle.Text}', {PositionRate.Text});";
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
                TextBox[] box = { PositionTitle, PositionRate };
                for (int i=0; i<box.Length; i++)
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

        private void PositionTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (PositionTitle.Text == "Invalid Input")
            {
                PositionTitle.Text = "";
                PositionTitle.Foreground = Brushes.Black;
            }
            return;
        }

        private void PositionRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (PositionRate.Text == "Invalid Input")
            {
                PositionRate.Text = "";
                PositionRate.Foreground = Brushes.Black;
            }
            return;
        }
    }
}
