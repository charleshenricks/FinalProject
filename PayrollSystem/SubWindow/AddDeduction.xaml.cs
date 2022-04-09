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
    /// Interaction logic for AddDeduction.xaml
    /// </summary>
    public partial class AddDeduction : Window
    {
        private string connection;
        private DeductionView main;
        public AddDeduction(string connection, DeductionView main)
        {
            InitializeComponent();
            this.connection = connection;
            this.main = main;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();

            if (!string.IsNullOrWhiteSpace(DeductionDescription.Text) && 
                !string.IsNullOrWhiteSpace(DeductionAmount.Text))
            {
                string command = $"INSERT INTO user_deduction VALUES ('{DeductionDescription.Text}', '{DeductionAmount.Text}');";
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
                TextBox[] box = { DeductionDescription, DeductionAmount };
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

        private void DeductionDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (DeductionDescription.Text == "Invalid Input")
            {
                DeductionDescription.Text = "";
                DeductionDescription.Foreground = Brushes.Black;
            }
            return;
        }
        
        private void DeductionAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (DeductionAmount.Text == "Invalid Input")
            {
                DeductionAmount.Text = "";
                DeductionAmount.Foreground = Brushes.Black;
            }
            return;
        }
    }
}
