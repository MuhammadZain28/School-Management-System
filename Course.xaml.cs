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
using System.Windows.Media.Effects;
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
    /// Interaction logic for Course.xaml
    /// </summary>
    public partial class Course : Page
    {
        List<ScheduleB> scheduleBs;
        public Course()
        {
            InitializeComponent();
            scheduleBs = new List<ScheduleB>();
            loadGrid();
            loadClass();
        }

        private void Admit_Copy_Click(object sender, RoutedEventArgs e)
        {

        }
        private void loadGrid()
        {
            ScheduleB scheduleB = new ScheduleB();
            scheduleBs = scheduleB.Schedules();
            SheduleGrid.ItemsSource = scheduleBs;
            
            CourseGrid.ItemsSource = scheduleBs;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void class_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Class());
        }
        private void view_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button?.DataContext is ResultB person)
            {
                int id = person.ResultId;
                int classid = person.Exams.Classes.classId;
                Mainframe.Navigate(new ResultEntry(0, classid, id));
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button?.DataContext is ResultB person)
            {
                int id = person.ResultId;
                int classid = person.Exams.Classes.classId;
                Mainframe.Navigate(new ResultEntry(0, classid, id, true));

            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button?.DataContext is ScheduleB person)
            {

                int id = person.ScheduleId;
                ScheduleB scheduleB = new ScheduleB();
                if (scheduleB.DeleteClass(id))
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

        private void loadClass()
        {
            ClassB classB = new ClassB();
            classes.ItemsSource = classB.load();
            classes.DisplayMemberPath = "Value";
            classes.SelectedValuePath = "Key";

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedClass = classes.SelectedItem as KeyValuePair<int, string>?;
            ScheduleB scheduleB = new ScheduleB();
            scheduleBs = scheduleB.FilterSchedule(selectedClass.Value.Key);
            CourseGrid.ItemsSource = null;
            SheduleGrid.ItemsSource = null;
            CourseGrid.ItemsSource = scheduleBs;
            SheduleGrid.ItemsSource = scheduleBs;
        }

        private void courses_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new AddCourse());
        }

        private void Mainframe_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Class_course());
        }
    }
}
