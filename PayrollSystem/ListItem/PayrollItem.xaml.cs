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

namespace PayrollSystem.ListItem
{
    /// <summary>
    /// Interaction logic for PayrollItem.xaml
    /// </summary>
    public partial class PayrollItem : UserControl
    {
        private PayrollView main;
        private string[] information;
        public PayrollItem(string [] information, PayrollView main)
        {
            InitializeComponent();
            this.main = main;
            this.information = information;
            EmployeeID.Content = information[0];
            EmployeeName.Content = information[1];
            EmployeeRatePerHour.Content = information[2];
            EmployeeDeduction.Content = information[3];
            EmployeeCashAdvance.Content = information[4];
            Compute();
            return;
        }

        private void EmployeeTimeWorked_KeyUp(object sender, KeyEventArgs e)
        {
            Compute();
            
            return;
        }

        private void Compute()
        {
            int time_worked = 0;
            int net_pay = 0;

            if (!string.IsNullOrWhiteSpace(EmployeeTimeWorked.Text))
            {
                time_worked = Convert.ToInt32(EmployeeTimeWorked.Text);
            }
            time_worked *= Convert.ToInt32(EmployeeRatePerHour.Content);
            EmployeeGross.Content = Convert.ToString(time_worked);

            net_pay = time_worked - (Convert.ToInt32(EmployeeDeduction.Content) + Convert.ToInt32(EmployeeCashAdvance.Content));
            EmployeeNetPay.Content = Convert.ToString(net_pay);
            return;
        }
    }
}
