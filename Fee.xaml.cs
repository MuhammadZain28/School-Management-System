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
using LMS.BL;
using LMS.DL;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Fee.xaml
    /// </summary>
    public partial class Fee : Page
    {
        private int feeId { get; set; }
        private bool IsSaved = false;
        public Fee(int id = -1, bool isUpdate = false)
        {
            InitializeComponent();
            feeId = id;
            if (id > 0)
            {
                if (isUpdate)
                    Update();
                else
                    view();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }
        private void AddData()
        {
            FeeB fee = inputData();
            if (feeId > 0)
            {
                if (fee.AddData(fee))
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Unsuccessful");
                }
            }
            else
            {
                if (fee.AddData(fee))
                {
                    MessageBox.Show("Update Successful");
                }
                else
                {
                    MessageBox.Show("Unsuccessful");
                }
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.No;
            if (!IsSaved)
            {
                result = MessageBox.Show("Want to save data", "Unsaved Work", MessageBoxButton.YesNo);
            }
            if (result == MessageBoxResult.Yes)
            {
                AddData();
            }
            MainFrame.Navigate(new Finance());
        }
        private FeeB inputData()
        {
            FeeB fee = new FeeB();
            fee.Name = name.Text;
            fee.roll = Roll.Text;
            fee.PaymentDate = Convert.ToDateTime(payDate.Text);
            int amount;
            if (int.TryParse(Paid.Text, out amount))
                fee.AmountPaid = amount;
            return fee;
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void view()
        {
            FeeB fee = new FeeB();
            fee = fee.search(feeId);
            name.Text = fee.Name;
            Roll.Text = fee.roll;
            due.Text = Convert.ToString(fee.DueAmount);
            Paid.Text = Convert.ToString(fee.AmountPaid);
            payDate.SelectedDate = fee.PaymentDate;
            disable();
        }

        private void Update()
        {
            FeeB fee = new FeeB();
            fee = fee.search(feeId);
            name.Text = fee.Name;
            Roll.Text = fee.roll;
            due.Text = Convert.ToString(fee.DueAmount);
            Paid.Text = Convert.ToString(fee.AmountPaid);
            payDate.SelectedDate = fee.PaymentDate;
            disable();

            Paid.IsEnabled = true;
            Submit.IsEnabled = true;
        }

        private void disable()
        {
            name.IsEnabled = false;
            Roll.IsEnabled = false;
            due.IsEnabled = false;
            Paid.IsEnabled = false;
            payDate.IsEnabled = false;
            Submit.IsEnabled = false;

        }
       
    }
}
