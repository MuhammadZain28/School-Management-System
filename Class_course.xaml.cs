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
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Class_course.xaml
    /// </summary>
    public partial class Class_course : Page
    {
        List<CourseB> courseList;
        List<ClassB> classList;
        public Class_course()
        {
            InitializeComponent();
            classList = new List<ClassB>();
            loadGrid();
        }

        private void Mainframe_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new AddCourse());
        }

        private void class_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new AddClass()); 
        }

        private void view_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is CourseB person)
            {

                CourseB courseB = new CourseB();
                if (courseB.Delete(person.courseID))
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
        private void loadGrid()
        {
            ClassB classB = new ClassB();   
            classList = classB.loadList();
            courseGrid.ItemsSource = classList;

            CourseB courseB = new CourseB();
            courseList = courseB.Get();
            ex.ItemsSource = courseList;
        }

        private void Class_Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is ClassB person)
            {

                ClassB classB = new ClassB();
                if (classB.deleteData(person.classId))
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
}
