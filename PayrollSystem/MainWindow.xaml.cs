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
using PayrollSystem.Views;
using PayrollSystem.Form;

namespace PayrollSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] information;
        string database;

        public MainWindow(string[] information)
        {
            InitializeComponent();
            this.information = information;
            string name = "";
            foreach (char x in information[0])
            {
                name += (!char.IsWhiteSpace(x))? $"{x}" : "_";
            }
            this.database = $"server=localhost; port=3306; user=root; password=password; database={name};";
            NameHere.Text = information[0];
            EmployeeView.connection = this.database;
            EmployeeView.ReloadListView();
            return;

        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }

            return;
        }

        private void ButtonSignOut(object sender, RoutedEventArgs e)
        {
            ChangeFont(SignOut);
            MyForm login = new MyForm();
            this.Close();
            login.Show();
            return;
        }

        private void ChangeWindow(UserControl view)
        {
            UserControl[] user = {EmployeeView, PositionView,
            CashAdvanceView, DeductionView, PayrollView,view};

            for (int i=0; i<user.Length; i++){
                user[i].Visibility = Visibility.Collapsed;
            }
          
            view.Visibility = Visibility.Visible;
            return;
        }

        private void ChangeFont(Button btn){
            var br = new BrushConverter();
            Button[] btns = {EmployeeButton, PositionButton,
            CashAdvanceButton, DeductionButton, PayrollButton, SignOut};

            for (int i=0; i<btns.Length ; i++){
                btns[i].Background = (Brush) br.ConvertFrom("#E7E8E9");
                btns[i].Foreground = Brushes.Black;
            }

            btn.Background =  (Brush) br.ConvertFrom("#1C658C");
            btn.Foreground = Brushes.White;
            return;
            
        }

        private void EmployeePage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(EmployeeView);
            ChangeFont(EmployeeButton);
            EmployeeView.connection = this.database;
            EmployeeView.ReloadListView();

            return;
        }

        private void PositionPage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(PositionView);
            ChangeFont(PositionButton);
            PositionView.connection = this.database;
            PositionView.ReloadListView();
            return;
        }

        private void CashAdvancePage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(CashAdvanceView);
            ChangeFont(CashAdvanceButton);
            CashAdvanceView.connection = this.database;
            CashAdvanceView.ReloadListView();
            return;
        }

        private void DeductionPage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(DeductionView);
            ChangeFont(DeductionButton);
            DeductionView.connection = this.database;
            DeductionView.ReloadListView();
            return;
        }

        private void PayrollPage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(PayrollView);
            ChangeFont(PayrollButton);
            PayrollView.connection = this.database;
            PayrollView.ReloadListView();
            return;
        }
    }
}
