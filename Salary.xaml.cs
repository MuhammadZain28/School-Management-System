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

namespace LMS
{
    /// <summary>
    /// Interaction logic for Salary.xaml
    /// </summary>
    public partial class Salary : Page
    {
        public string date {  get; set; }
        public bool isPrevious = false;
        public Salary()
        {
            InitializeComponent();
            date = DateTime.Now.ToString("MM");
            month.ItemsSource = loadMonth();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hire_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Mainframe_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void calc_Click(object sender, RoutedEventArgs e)
        {
            SalaryB salary = new SalaryB();
            if (!isPrevious)
            {
                salary.calculateSalary();
            }
            data.ItemsSource = null;
            data.ItemsSource = salary.GetSalaries(date);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox?.DataContext is SalaryB salary)
            {
                SalaryD.Update(salary.salary_id, checkbox.IsChecked == true, DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMonth = month.SelectedIndex + 1;
            if (selectedMonth < 10)
            {
                date = "0" + selectedMonth;
            }
            else
            {
                date = "" + selectedMonth;
            }
            isPrevious = false;
            if (selectedMonth == DateTime.Now.Month)
                isPrevious = false;
        }
        private Dictionary<int, string> loadMonth()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                months.Add(i, monthName);
            }

            month.ItemsSource = months;
            month.DisplayMemberPath = "Value";
            month.SelectedValuePath = "Key";
            return months;
        }

        private void teacher_click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Teacher());
        }
    }
}
