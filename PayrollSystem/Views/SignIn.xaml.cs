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
using PayrollSystem;
using MySql.Data.MySqlClient;

namespace PayrollSystem.Views
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void UserBox_KeyDown(object sender, KeyEventArgs e){
            if (UserBox.Text == "Username" || UserBox.Text == "Invalid Input"){
                UserBox.Text = "";
                UserBox.Foreground = Brushes.Black;
            }

            return;
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e){
            if (PasswordBox.Password == "Password" || PasswordBox.Password == "Invalid Input"){
                PasswordBox.Password = "";
                PasswordBox.Foreground = Brushes.Black;
            }
            return;
        }

        private void ResetBox(object sender, KeyEventArgs e){
            var br = new BrushConverter();
            if (UserBox.Text == ""){
                UserBox.Text = "Username";
                UserBox.Foreground = (Brush) br.ConvertFrom("#AAA4A4");
            }

            if (PasswordBox.Password == ""){
                PasswordBox.Password = "Password";
                PasswordBox.Foreground = (Brush) br.ConvertFrom("#AAA4A4");
            }
            return;
        }

        private void SignInButton(object sender, RoutedEventArgs e){

            if (UserBox.Text == "Username"){
                UserBox.Text = "Invalid Input";
                UserBox.Foreground = Brushes.Red;
            }

            if (PasswordBox.Password == "Password"){
                PasswordBox.Password = "Invalid Input";
                PasswordBox.Foreground = Brushes.Red;
            }

            if (UserBox.Text != "Username" && PasswordBox.Password != "Password"){
                string database = "server=localhost; port=3306; user=root; password= password; database=payroll_system_admin;";
                string command = "";
                
                MySqlConnection conn = new MySqlConnection(database);
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataReader rdr;

                try {
                    conn.Open();

                    command = $"SELECT * FROM ACCOUNT WHERE ACCOUNT_USERNAME = '{UserBox.Text}';";
                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    rdr = cmd.ExecuteReader();

                    rdr.Read();
                    string[] information = {rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString()};
                    rdr.Close();

                    if (information[1] == UserBox.Text && information[2] == PasswordBox.Password){
                        MainWindow main = new MainWindow(information);
                        var old = Window.GetWindow(this);
                        
                        old.Hide();
                        main.Show();
                    }
                    
                } catch (Exception err){
                    MessageBox.Show(err.Message);
                } finally{
                    conn.Close();
                }
                
            }

            return;
            
        }
    }
}
