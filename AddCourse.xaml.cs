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
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourse : Page
    {
        public AddCourse()
        {
            InitializeComponent();
            loadCombo();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void loadCombo()
        {
            TeacherB teacherB = new TeacherB();
            teachers.ItemsSource = teacherB.GetTeacherCombo();
            teachers.DisplayMemberPath = "Value";
            teachers.SelectedValuePath = "Key";

            Type.Items.Add("Regular");
            Type.Items.Add("Short");
        }
        private CourseB InputData()
        {
            var selectedTeacher = teachers.SelectedItem as KeyValuePair<int, string>?;
            CourseB courseB;
            courseB = new CourseB()
            {
                courseName = name.Text,
                TeacherId = selectedTeacher.Value.Key,
                type = Type.Text,
            };
            MessageBox.Show("" + courseB.type);
            return courseB;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            CourseB courseB = InputData();
            if (courseB.Insert(courseB))
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Unsuccessfully");
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Course());
        }
    }
}
