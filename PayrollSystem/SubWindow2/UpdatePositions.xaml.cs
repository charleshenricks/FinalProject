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
    /// Interaction logic for UpdatePositions.xaml
    /// </summary>
    public partial class UpdatePositions : Window
    {
        private string connection;
        private string old_item;
        private PositionView main;

        public UpdatePositions(string[] information, string connection, PositionView main )
        {
            InitializeComponent();
            TextBox[] box = { PositionTitle, PositionRate };
            for (int i = 0; i < box.Length; i++)
            {
                box[i].Text = information[i];
            }
            this.connection = connection;
            this.old_item = PositionTitle.Text;
            this.main = main;
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

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();

            if (!(string.IsNullOrWhiteSpace(PositionTitle.Text)) &&
                !(string.IsNullOrWhiteSpace(PositionRate.Text)))
            {
                string command = $"UPDATE user_position SET POSITION_TITLE = '{PositionTitle.Text}', POSITION_RATE_PER_HOUR = {PositionRate.Text} WHERE POSITION_TITLE = '{this.old_item}';";
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
                for (int i = 0; i < box.Length; i++)
                {
                    box[i].Text = "Invalid Input";
                    box[i].Foreground = Brushes.Red;
                }
            }
            this.main.ReloadListView();
            this.Close();
        }
    }
}
