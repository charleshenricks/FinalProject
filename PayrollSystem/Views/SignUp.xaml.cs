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
using System.IO;
using MySql.Data.MySqlClient;

namespace PayrollSystem.Views
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : UserControl
    {

        private MySqlConnection conn;
        private MySqlCommand cmd;

        public SignUp()
        {
            InitializeComponent();
            string database = "server=localhost; port=3306; user=root; password= password; database=payroll_system_admin;";
            this.conn = new MySqlConnection(database);
            this.cmd = new MySqlCommand();
            return;
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e){
            if (NameBox.Text == "Name" || NameBox.Text == "Invalid Input"){
                NameBox.Text = "";
                NameBox.Foreground = Brushes.Black;
            }

            return;
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
            if (NameBox.Text == ""){
                NameBox.Text = "Name";
                NameBox.Foreground = (Brush) br.ConvertFrom("#AAA4A4");
            }

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

        private void SignUpButton(object sender, RoutedEventArgs e){

            if (NameBox.Text == "Name"){
                NameBox.Text = "Invalid Input";
                NameBox.Foreground = Brushes.Red;
            }
            if (UserBox.Text == "Username"){
                UserBox.Text = "Invalid Input";
                UserBox.Foreground =  Brushes.Red;
            }

            if (PasswordBox.Password == "Password"){
                PasswordBox.Password = "Invalid Input";
                PasswordBox.Foreground = Brushes.Red;
            }

            if (NameBox.Text != "Name" && UserBox.Text != "Username" && 
                PasswordBox.Password != "Password"){
                    string [] information = {NameBox.Text, UserBox.Text, PasswordBox.Password};
                    string command = "";

                    try {
                        this.conn.Open();

                        command = $"INSERT INTO account VALUES ('{NameBox.Text}',"
                                 +$"'{UserBox.Text}', '{PasswordBox.Password}');";
                        
                        this.cmd.Connection = this.conn;
                        this.cmd.CommandText = command;
                        this.cmd.ExecuteNonQuery();

                        createDatabase(information);
                        MainWindow main = new MainWindow(information);
                        var old = Window.GetWindow(this);
                        
                        old.Hide();
                        main.Show();

                    } catch (Exception err){
                        MessageBox.Show(err.Message);
                    } finally {
                        this.conn.Close();
                    }
                
            }
            return;
        }

        private void createDatabase(string [] information){
            string database = "server=localhost;port=3306;user=root; password= password;";
            MySqlConnection myConn = new MySqlConnection(database);
            MySqlCommand myCmd = new MySqlCommand();
            string command = "";

            string name = "";
            foreach (char x in information[0]){
                name += (!char.IsWhiteSpace(x)) ? $"{x}" : "_";
            }

            myConn.Open();
            command = $"CREATE DATABASE {name};";
            myCmd.Connection = myConn;
            myCmd.CommandText = command;
            myCmd.ExecuteNonQuery();
            myConn.Close();

            try
            {
                database += $"database = {name};";
                myConn = new MySqlConnection(database);
                myCmd.Connection = myConn;
                myConn.Open();

                string pathName = @"E:\C#\PayrollSystem\PayrollSystem\PayrollSystem\Views\USER_DATABASE.txt";
                string[] fileLine = File.ReadAllLines(pathName);
                string line = "";

                foreach (string x in fileLine)
                {
                    if (x != "")
                    {
                        line += x;
                    }
                    else
                    {
                        myCmd.CommandText = line;
                        myCmd.ExecuteNonQuery();
                        line = "";
                    }
                }

                command = $"INSERT INTO USER_ACCOUNT VALUES('{information[0]}','{information[1]}', '{information[2]}');";
                myCmd.CommandText = command;
                myCmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            } 
            finally
            {
                myConn.Close();
            }
            return;
        }
    }
}
