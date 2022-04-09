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
    /// Interaction logic for UpdateDeductions.xaml
    /// </summary>
    public partial class UpdateDeductions : Window
    {
        private string connection;
        private string old_item;
        private DeductionView main;

        public UpdateDeductions(string[] information, string connection, DeductionView main)
        {
            InitializeComponent();
            TextBox[] box = { DeductionDescription, DeductionAmount };
            for (int i = 0; i < box.Length; i++)
            {
                box[i].Text = information[i];
            }
            this.connection = connection;
            this.old_item = DeductionDescription.Text;
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

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(this.connection);
            MySqlCommand cmd = new MySqlCommand();

            if (!(string.IsNullOrWhiteSpace(DeductionDescription.Text)) &&
                !(string.IsNullOrWhiteSpace(DeductionAmount.Text)))
            {
                string command = $"UPDATE user_deduction SET DEDUCTION_DESCRIPTION = '{DeductionDescription.Text}', DEDUCTION_AMOUNT = {DeductionAmount.Text} WHERE DEDUCTION_DESCRIPTION = '{this.old_item}';";
    
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
                this.main.ReloadListView();
                this.Close();
            }
            else
            {
                TextBox[] box = { DeductionDescription, DeductionAmount };
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
