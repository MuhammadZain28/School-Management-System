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
using MahApps.Metro.Controls;
using LMS.BL;
using LMS.DL;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Class.xaml
    /// </summary>
    public partial class Class : Page
    {
        private HashSet<ScheduleB> scheduleBs;
        public Class()
        {
            InitializeComponent();
            loadClass();
            scheduleBs = new HashSet<ScheduleB>();
            teachers.IsEnabled = false; 
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (scheduleBs != null)
            {
                ScheduleB scheduleB = new ScheduleB();
                if (NotNull())
                {
                    scheduleBs.Add(InputData());
                }
                if (scheduleB.AddSchedule(scheduleBs))
                {
                    MessageBox.Show("Success");
                    scheduleBs.Clear();
                }
                else
                {
                    MessageBox.Show("Unsuccessful");
                }
            }
            else
            {
                MessageBox.Show("No Class Added!");
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void loadClass()
        {
            ClassB classB = new ClassB();
            @class.ItemsSource = classB.load();
            @class.DisplayMemberPath = "Value";
            @class.SelectedValuePath = "Key";

            CourseB courseB = new CourseB();    
            subject.ItemsSource = courseB.loadBatch();
            subject.DisplayMemberPath = "Value";
            subject.SelectedValuePath = "Key";

            List<string> days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            day.ItemsSource = days;

        }

        private void subject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            teachers.IsEnabled = true;
            tea.Content = "Teacher";
            var selectedClass = subject.SelectedItem as KeyValuePair<int, string>?;
            TeacherB teacherB = new TeacherB();
            teachers.ItemsSource = teacherB.GetTeachersbyCourse(selectedClass.Value.Key);
            teachers.DisplayMemberPath = "Value";
            teachers.SelectedValuePath = "Key";
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (NotNull())
            {
                scheduleBs.Add(InputData());
                day.Text = "";
                start.SelectedDateTime = null;
                end.SelectedDateTime = null;
                teachers.Text = null;
                MessageBox.Show("Class Added!");
            }
            else
            {
                MessageBox.Show("All Fields are required");
            }
        }
        private ScheduleB InputData()
        {
            ScheduleB scheduleB = new ScheduleB();
            var selectedClass = @class.SelectedItem as KeyValuePair<int, string>?;
            var selectedSubject = subject.SelectedItem as KeyValuePair<int, string>?;
            var selectedTeacher = teachers.SelectedItem as KeyValuePair<int, string>?;
            
            scheduleB = new ScheduleB()
            {
                Classes = new ClassB()
                {
                    classId = selectedClass.Value.Key,
                    name = selectedClass.Value.Value,
                },
                Teachers = new TeacherB()
                {
                    id = selectedTeacher.Value.Key,
                    name = selectedTeacher.Value.Value,
                },
                Courses = new CourseB()
                {
                    courseID = selectedSubject.Value.Key,
                    courseName = selectedSubject.Value.Value,
                },
                DayOfWeek = day.Text,
                StartTime = start.SelectedDateTime?.TimeOfDay ?? TimeSpan.Zero,
                EndTime = end.SelectedDateTime?.TimeOfDay ?? TimeSpan.Zero,
            };
            
            return scheduleB;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Course());
        }
        private bool NotNull()
        {
            var selectedClass = @class.SelectedItem as KeyValuePair<int, string>?;
            var selectedSubject = subject.SelectedItem as KeyValuePair<int, string>?;
            var selectedTeacher = teachers.SelectedItem as KeyValuePair<int, string>?;
            var selectedDay = day.Text;
            TimeSpan selectedStartTime = start.SelectedDateTime?.TimeOfDay ?? TimeSpan.Zero;
            TimeSpan selectedEndTime = end.SelectedDateTime?.TimeOfDay ?? TimeSpan.Zero;

            if (selectedClass.HasValue && selectedSubject.HasValue && selectedTeacher.HasValue &&
                !string.IsNullOrEmpty(selectedDay) && selectedStartTime != null && selectedEndTime != null)
            {
                ScheduleB scheduleB = new ScheduleB();
                
                return scheduleB.checkStartEnd(selectedStartTime, selectedEndTime, selectedSubject.Value.Key, selectedDay);
            }
            else 
                return false;
        }
    }
}
