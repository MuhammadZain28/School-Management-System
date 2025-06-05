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
using OxyPlot.Series;
using OxyPlot;
using LMS.BL;
using System.Xml.Linq;
using LMS.DL;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Finance.xaml
    /// </summary>
    public partial class Finance : Page
    {
        public PlotModel PieChart {  get; set; }
        public int trueCount { get; set; } 
        public int falseCount { get; set; }
        public bool IsSearch { get; set; }
        List<FeeB> fees;
        public Finance(string role = " ")
        {
            InitializeComponent();

            DataContext = this;
            loadGrid();
            PaidCount();
            fees = new List<FeeB>();
            PieChart = FeePie();
            Month.IsEnabled = false;
            IsSearch = false;
            if (role == "coordinator")
            {
                feeborder.Opacity = 0;
                expborder.Opacity = 0;
                expense.Opacity = 0;
                expense.IsEnabled = false;
            }
        }

        private void Admit_Copy_Click(object sender, RoutedEventArgs e)
        {

        }
        private void PaidCount()
        {
            if (fees.Count(f => f.Status == true) != null) 
                trueCount = fees.Count(f => f.Status == true);
            else
                trueCount = 0;
            if (fees.Count(f => f.Status == false) != null)
                falseCount = fees.Count(f => f.Status == false);
            else
                falseCount = 0;
        }
        private PlotModel FeePie()
        {
            var model = new PlotModel { };
            var pieSeries = new PieSeries
            {
                StrokeThickness = 0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0,
                InnerDiameter = 0.6
            };

            pieSeries.Slices.Add(new PieSlice("", trueCount)
            {
                Fill = OxyColor.FromArgb(255, 91, 230, 88)
            });

            pieSeries.Slices.Add(new PieSlice("", falseCount)
            {
                Fill = OxyColor.FromArgb(255, 247, 90, 90) 
            });

            model.Series.Add(pieSeries);
            return model;
        }

        private void MonthFilter_Click(object sender, RoutedEventArgs e)
        {
            Month.IsEnabled = true;
            load();
        }

        private void Fee_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate (new Fee());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void loadGrid()
        {
            FeeB feeB = new FeeB();
            fees = feeB.getFees();

            feeGrid.ItemsSource = fees;

            amount.Content = "Rs. " + feeB.Total();
        }

        private void view_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is FeeB person)
            {
                int id = person.FeeId;
                MainFrame.Navigate(new Fee(id));
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is FeeB person)
            {
                int id = person.FeeId;
                MainFrame.Navigate(new Fee(id, true));

            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button?.DataContext is FeeB person)
            {
                int id = person.stdId;
                FeeB fee = new FeeB();
                fee = inputData();
                fee.stdId = id;
                if (fee != null)
                {
                    if (fee.deleteData(fee))
                    {
                        MessageBox.Show("Success");
                        loadGrid();
                    }
                    else
                    {
                        MessageBox.Show("Unsuccessful");
                    }
                }
            }

        }
        private void searchFee()
        {
            loadGrid();
            string nm = search.Text;
            var stdFee = fees.FindAll(t => t.roll == nm);
            foreach (var item in stdFee)
            {
            }
            if (!IsSearch)
            {
                feeGrid.ItemsSource = null;
                feeGrid.ItemsSource = stdFee;
            }
            else
            {
                feeGrid.ItemsSource = null;
                feeGrid.ItemsSource = fees;
            }
            IsSearch = !IsSearch;
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            searchFee();
        }
        private FeeB inputData()
        {
            FeeB fee = new FeeB();
            fee.PaymentDate = DateTime.Now;
            fee.AmountPaid = 0;
            return fee;
        }

        private void search_GotFocus(object sender, RoutedEventArgs e)
        {
            search.Text = "";
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
            FeeB feeB = new FeeB();
            fees = feeB.filter(selectedPair.Value.Key);

            feeGrid.ItemsSource = fees;

            amount.Content = "Rs. " + feeB.filterFee(selectedPair.Value.Key);
            PaidCount();
            PieChart = FeePie();
        }

        private void expense_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Expense());
        }
    }
}
