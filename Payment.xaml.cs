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
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Page
    {
        public int PID { get; set; }
        public bool IsSaved = false;
        public Payment(int id = -1)
        {
            InitializeComponent();
            loadCombo();
            PID = id;
            if (PID > 0)
            {
                View();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }
        private void AddData()
        {
            ExpenseB expenseB = GetExpense();
            if (expenseB.add(expenseB))
            {
                MessageBox.Show("Successfull");
            }
            else
            {
                MessageBox.Show("Unsuccessful");
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
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
            MainFrame.Navigate(new Expense());
        }
        private ExpenseB GetExpense()
        {
            ExpenseB expenseB = new ExpenseB();
            expenseB.Name = Name.Text;
            expenseB.Contact = ID.Text;
            expenseB.PaymentDate = Convert.ToDateTime(paydate.SelectedDate);
            if (Decimal.TryParse(Amount.Text, out Decimal parsedAmount))
            {
                expenseB.Amount_paid = parsedAmount;
            }
            else
            {
                MessageBox.Show("Invalid amount entered. Please enter a valid number.");
            }
            expenseB.ToEntity = (string)to.SelectedValue;
            expenseB.PaymentType = (string)type.SelectedValue;
            return expenseB;
        }
        private void loadCombo()
        {
            type.Items.Add("Salary");
            type.Items.Add("Bill");
            type.Items.Add("Rent");
            type.Items.Add("Other");
            type.SelectedValuePath = "Value";

            to.Items.Add("Teacher");
            to.Items.Add("Government");
            to.Items.Add("Other");
            to.SelectedValuePath = "Value";
        }
        private void View()
        {
            ExpenseB expenseB = new ExpenseB();
            expenseB = expenseB.GetExpense(PID);
            Name.Text = expenseB.Name;
            ID.Text = expenseB.Contact;
            paydate.SelectedDate = (DateTime)(expenseB.PaymentDate);
            Amount.Text = Convert.ToString(expenseB.Amount_paid);
            to.SelectedValue = expenseB.ToEntity;
            type.SelectedValue = expenseB.PaymentType;


            disable();
        }
        private void disable()
        {
            Name.IsEnabled = false; 
            ID.IsEnabled = false; 
            paydate.IsEnabled = false; 
            Amount.IsEnabled = false;
            to.IsEnabled = false;
            type.IsEnabled = false;
        }
    }
}
