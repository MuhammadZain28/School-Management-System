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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using LMS.BL;
using LMS.DL;

namespace LMS
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Page
    {
        public string role {  get; set; }
        public PlotModel PieModel { get; set; }
        public PlotModel PieModel1 { get; set; }
        public PlotModel LineGraphModel { get; set; }
        public PlotModel BarModel { get; set; }
        private List<ResultB> resultBs;
        public DashBoard(string role = "")
        {
            InitializeComponent();
            ShowTotalStudent();
            date.DisplayDate = DateTime.Now;

            DataContext = this;

            PieModel = studentPie();
            LineGraphModel = CreateLineGraph();
            BarModel = CreateHorizontalBarChart(); 

            TeacherAttendenceB teacher = new TeacherAttendenceB();
            teacher.Log();

            AttendenceB attendenceB = new AttendenceB();
            int present = attendenceB.count('P', DateTime.Now);
            int Absent = attendenceB.count('A', DateTime.Now);
            int leave = attendenceB.count('L', DateTime.Now);

            bgNot.Opacity = 0;

            PieModel1 = AttendencePie(present, Absent, leave);

            resultBs = new List<ResultB>();
            loadCombo();
            showEvent();
            rolelabel.Content = "Admin";

            this.role = role;
            if (role == "coordinator")
            {
                batch.IsEnabled = false;
                batch.Opacity = 0;
                rolelabel.Content = "Coordinator";
                noti.IsEnabled = false;
                noti.Opacity = 0;
            }
            else
            {
                notices.Content = " " + NotificationD.Instance.Count();
                if (NotificationD.Instance.Count() > 0)
                    bgNot.Opacity = 1;
            }
        }
        public PlotModel studentPie()
        {
            StudentB _student = new StudentB();
            var model = new PlotModel { };
            var pieSeries = new PieSeries
            {
                StrokeThickness = 0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0,
                InnerDiameter = 0.6
            };

            pieSeries.Slices.Add(new PieSlice("", 70)
            {
                Fill = OxyColor.FromRgb(135, 206, 235)
            });

            pieSeries.Slices.Add(new PieSlice("", 30)
            {
                Fill = OxyColor.FromRgb(255, 165, 0)
            });

            model.Series.Add(pieSeries);
            return model;
        }
        public PlotModel CreateLineGraph()
        {
            FeeB fee = new FeeB();
            List<FeeB> fees = fee.getGraph();
            
            var model = new PlotModel { };
            
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title= "MONTH" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left});

            var lineSeries = new LineSeries
            {
                Title = "Data",
                Color = OxyColor.FromArgb(255, 244, 149, 82),
                StrokeThickness = 2,
            };

            foreach (var item in fees)
            {
                lineSeries.Points.Add(new DataPoint(item.month, item.AmountPaid));
            }

            model.Series.Add(lineSeries);


            return model;
        }
        private PlotModel AttendencePie(int p = 1, int a = 2, int l = 3)
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

            pieSeries.Slices.Add(new PieSlice("", p)
            {
                Fill = OxyColor.FromArgb(255, 0, 255, 0)
            });

            pieSeries.Slices.Add(new PieSlice("", a)
            {
                Fill = OxyColor.FromRgb(255, 0, 0)
            });

            pieSeries.Slices.Add(new PieSlice("", l)
            {
                Fill = OxyColor.FromRgb(255, 255, 0)
            });
            model.Series.Add(pieSeries);
            return model;
        }
        private PlotModel CreateHorizontalBarChart()
        {
            var model = new PlotModel { Title = "SUBJECT PERFORMANCE" };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CategoryAxis",
                ItemsSource = new[] { "Apples", "Bananas", "Cherries", "Dates", "Elderberries" },
                IsPanEnabled = false,
                IsZoomEnabled = false
            };
            model.Axes.Add(categoryAxis);

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            });

            var barSeries = new BarSeries
            {
                ItemsSource = new List<BarItem>
                {
                    new BarItem { Value = 25 },
                    new BarItem { Value = 40 },
                    new BarItem { Value = 18 },
                    new BarItem { Value = 30 },
                    new BarItem { Value = 22 }
                },
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                FillColor = OxyColor.FromRgb(122, 116, 220)
            };

            model.Series.Add(barSeries);
            return model;
        }
        private void ShowTotalStudent()
        {
            StudentB TotalStudent = new StudentB();
            student.Content = Convert.ToString(TotalStudent.totalStudents());

            TeacherB Totalteacher = new TeacherB();
            teacher.Content = Convert.ToString(Totalteacher.Total());

            FeeB fees = new FeeB();
            revenue.Content = "Rs. " + Convert.ToString(fees.Total());

            ExpenseB _expense = new ExpenseB();
            expense.Content = "Rs. " + Convert.ToString(_expense.Total());
        }
        private void showEvent()
        {
            EventB Event = new EventB();    
            List<EventB> eventBs = new List<EventB>();
            eventBs = Event.getData();
            events.ItemsSource = eventBs;

            ResultB resultB = new ResultB();
            resultBs = resultB.GetResults();
            Top.ItemsSource = resultBs;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (role != "coordinator")
                MainFrame.Navigate(new Announcement());
            else
                MainFrame.Navigate(new coAnnouncements());
        }
        private void loadCombo()
        {
            ClassB classB = new ClassB();
            @class.ItemsSource = classB.load();
            @class.DisplayMemberPath = "Value";
            @class.SelectedValuePath = "Key";
        }
        private void batch_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate (new Batches());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedClass = @class.SelectedItem as KeyValuePair<int, string>?;
            ResultB resultB = new ResultB();
            resultBs = resultB.FilterResult(selectedClass.Value.Key);
            Top.ItemsSource = null;
            Top.ItemsSource = resultBs;
        }

        private void noti_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Notification());
        }
    }
}
