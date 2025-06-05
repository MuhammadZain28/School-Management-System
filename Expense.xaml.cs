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
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Expense.xaml
    /// </summary>
    public partial class Expense : Page
    {
        List<ExpenseB> expenses;
        public bool IsSearch {  get; set; }
        public Expense()
        {
            InitializeComponent();
            expenses = new List<ExpenseB>();
            load_data();
            Month.IsEnabled = false;
            IsSearch = false;
        }

        private void Fee(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Finance());
        }

        private void payment_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate (new Payment());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void load_data()
        {
            expenses.Clear();
            ExpenseB expenseB = new ExpenseB();
            expenses = expenseB.ExpenseList();

            ExpenseGrid.ItemsSource = null;
            ExpenseGrid.ItemsSource = expenses;

            exp.Content = "Rs. " + expenseB.Total();
        }
        private void view_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is ExpenseB person)
            {
                int id = person.PaymentID;
                MainFrame.Navigate(new Payment(id));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is ExpenseB person)
            {
                int id = person.PaymentID;
                if (person.remove(id))
                {
                    MessageBox.Show("Deleted SuccessFully");

                    load_data();
                }
                else
                {
                    MessageBox.Show("UnsuccessFul");
                }
            }
        }
        private Dictionary<int, string> load()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                months.Add(i, monthName);
            }

            Month.ItemsSource = months;
            Month.DisplayMemberPath = "Value";
            Month.SelectedValuePath = "Key";
            return months;
        }

        private void Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPair = Month.SelectedItem as KeyValuePair<int, string>?;
            ExpenseB expenseB = new ExpenseB();
            expenses = expenseB.FilterList(selectedPair.Value.Key);

            ExpenseGrid.ItemsSource = expenses;

            exp.Content = "Rs. " + expenseB.filter(selectedPair.Value.Key);
        }

        private void searchExp()
        {
            load_data();
            string nm = search.Text;
            var expensebs = expenses.FindAll(t => t.Contact.Contains(nm));
            foreach (var item in expensebs)
            {
                MessageBox.Show("" + item.Contact);
            }
            if (!IsSearch)
            {
                ExpenseGrid.ItemsSource = null;
                ExpenseGrid.ItemsSource = expensebs;
            }
            else
            {
                ExpenseGrid.ItemsSource = null;
                ExpenseGrid.ItemsSource = expenses;
            }
            IsSearch = !IsSearch;
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            searchExp();
        }
        private void fil_Click(object sender, RoutedEventArgs e)
        {
            load();
            Month.IsEnabled = true;
        }
        private void search_GotFocus(object sender, RoutedEventArgs e)
        {
            search.Text = "";
        }
    }
}
