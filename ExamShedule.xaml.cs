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
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for ExamShedule.xaml
    /// </summary>
    public partial class ExamShedule : Page
    {
        private int ID {  get; set; }
        private bool Isupdate { get; set; }
        private bool IsSaved = false;
        public ExamShedule(int id = -1, bool IsU = false)
        {
            InitializeComponent();
            load();
            ID = id;
            Isupdate = IsU;
            if (ID > 0)
            {
                if (IsU)
                    Update();
               else 
                    View();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }
        private void AddData()
        {
            if (!Isupdate)
            {
                ExamB exam = inputData();
                if (exam.AddExam(exam))
                {
                    MessageBox.Show("Success");
                }
                else
                    MessageBox.Show("Unsuccessful");
            }
            else
            {
                ExamB exam = inputData();
                if (exam.UpdateExam(exam, ID))
                {
                    MessageBox.Show("Success");
                }
                else
                    MessageBox.Show("Unsuccessful");
            }
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private ExamB inputData()
        {
            ExamB exam = new ExamB();

            var selectedClass = @class.SelectedItem as KeyValuePair<int, string>?;
            var selectedSubject = course.SelectedItem as KeyValuePair<int, string>?;
            exam.Classes.classId = selectedClass.Value.Key;
            exam.Courses.courseID = selectedSubject.Value.Key;
            exam.Date = (DateTime)date.SelectedDate;
            exam.TotalMarks = Convert.ToInt32(mark.Text);
            exam.Duration = duration.Text;
            return exam;
        }
        private void load()
        {
            ClassB classB = new ClassB();
            @class.ItemsSource = classB.load();
            @class.DisplayMemberPath = "Value";
            @class.SelectedValuePath = "Key";

            CourseB courseB = new CourseB();
            course.ItemsSource = courseB.loadBatch();
            course.DisplayMemberPath = "Value";
            course.SelectedValuePath = "Key";
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
            MainFrame.Navigate(new Exam());
        }
        private void View()
        {
            ExamB exam = new ExamB();
            exam = exam.SearchExam(ID);
            date.SelectedDate = (DateTime)exam.Date;
            duration.Text = exam.Duration;
            mark.Text = exam.TotalMarks.ToString();
            @class.Text = exam.className;
            course.Text = exam.CourseName;
            Disable();
        }
        private void Update()
        {
            View();
            date.IsEnabled = true;
            duration.IsEnabled = true;
            mark.IsEnabled = true;
            Submit.IsEnabled = true;

        }
        private void Disable()
        {
            date.IsEnabled = false;
            duration.IsEnabled = false;
            mark.IsEnabled = false;
            @class.IsEnabled = false;
            course.IsEnabled = false;
            Submit.IsEnabled = false;
        }

    }
}
