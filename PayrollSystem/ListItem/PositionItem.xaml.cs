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
using PayrollSystem.Views;
using PayrollSystem.SubWindow2;

namespace PayrollSystem.ListItem
{
    /// <summary>
    /// Interaction logic for PositionItem.xaml
    /// </summary>
    public partial class PositionItem : UserControl
    {
        private string [] information;
        private string connection;
        private PositionView main;
        public PositionItem(string[] information, string connection, PositionView main)
        {
            InitializeComponent();
            Label[] lbl = { PositionLabel, RateLabel };
            for (int i=0; i<lbl.Length; i++)
            {
                lbl[i].Content = information[i];
            }
            this.information = information;
            this.connection = connection;
            this.main = main;
            return;
        }

        private void EditButton(object sender, RoutedEventArgs e)
        {
            UpdatePositions position = new UpdatePositions(this.information, this.connection, this.main);
            position.Show();
            return;
        }

        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn.Open();
                string command = $"DELETE FROM user_position WHERE POSITION_TITLE = '{PositionLabel.Content}';";
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
            this.main.ReloadListView();
            return;
        }
    }
}
