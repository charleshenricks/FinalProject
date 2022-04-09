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

namespace PayrollSystem.Form
{
    /// <summary>
    /// Interaction logic for MyForm.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        public MyForm()
        {
            InitializeComponent();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
            return;
        }

        private void ChangeWindow(UserControl view)
        {
            SignInView.Visibility = Visibility.Collapsed;
            SignUpView.Visibility = Visibility.Collapsed;
            view.Visibility = Visibility.Visible;
            return;
        }
        private void SignInPage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(SignInView);
            return;
        }
    
        private void SignUpPage(object sender, RoutedEventArgs e)
        {
            ChangeWindow(SignUpView);
            return;
        }

        private void ExitWindow(object sender, RoutedEventArgs e){
            this.Close();
            return;
        }
    }
}
