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
using Google.Protobuf.Compiler;
using LMS.BL;
using MahApps.Metro.Controls.Dialogs;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class Teacher : Page
    {
        private List<TeacherB> teacherBs;
        private bool isSearch { get; set; }
        public Teacher()
        {
            InitializeComponent();
            teacherBs = new List<TeacherB>();
            teacher_data();        
            isSearch = false;
        }
        private void hire_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Hire());
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is TeacherB person)
            {
                int id = person.id;
                Mainframe.Navigate(new Hire(id));
            }
        }
        private bool teacher_data()
        {

            teacherBs.Clear();
            TeacherB teachers = new TeacherB();
            teacherBs = teachers.getData();

            data.ItemsSource = null;
            data.ItemsSource = teacherBs;
            return true;
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is TeacherB person)
            {
                int id = person.id;
                if (person.deleteData(id))
                {
                    MessageBox.Show("Deleted SuccessFully");

                    teacher_data();
                }
                else
                {
                    MessageBox.Show("UnsuccessFul");
                }
            }
        }
        private void Mainframe_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            List<TeacherB> teacherb = new List<TeacherB>();
            TeacherB teacher = new TeacherB();
            string nm = search.Text;
            teacherb = teacherBs.FindAll(t => t.name.Contains(nm));
            if (!isSearch)
            {
                data.ItemsSource = null;
                data.ItemsSource = teacherb;
            }
            else
            {
                data.ItemsSource = null;
                data.ItemsSource = teacherBs;
            }
            isSearch = !isSearch;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Salary());
        }
    }
}
