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
    /// Interaction logic for Exam.xaml
    /// </summary>
    public partial class Exam : Page
    {
        List<ExamB> examList;
        string Role;
        public Exam(string role = " ")
        {
            InitializeComponent();
            examList = new List<ExamB>();
            load_grid();
            loadClass();
            Role = role;
            if (role == "coordinator")
            {
                Schedule.Opacity = 0;
                Schedule.IsEnabled = false;
            }
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is ExamB person)
            {
                int id = person.ExamId;
                int classId = person.Classes.classId;
                Mainframe.Navigate(new ResultEntry(id,classId));
            }
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new ExamShedule());
        }
        private void load_grid()
        {
            examList.Clear();
            ExamB examB = new ExamB();
            examList = examB.GetExams();
            ex.ItemsSource = null;
            ex.ItemsSource = examList;

        }
        private void view_Click(object sender, RoutedEventArgs e)
        {
            if (Role != "coordinator")
            {
                Button button = sender as Button;
                if (button?.DataContext is ExamB person)
                {
                    int id = person.ExamId;
                    Mainframe.Navigate(new ExamShedule(id));
                }
            }
            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Role != "coordinator")
            {
                Button button = sender as Button;
                if (button?.DataContext is ExamB person)
                {
                    int id = person.ExamId;
                    Mainframe.Navigate(new ExamShedule(id, true));

                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Role != "coordinator")
            {
                Button button = sender as Button;
                if (button?.DataContext is ExamB person)
                {

                    int id = person.ExamId;
                    ExamB examB = new ExamB();
                    if (examB.DeleteExam(id))
                    {
                        MessageBox.Show("Success");
                        load_grid();
                    }
                    else
                    {
                        MessageBox.Show("Unsuccessful");
                    }


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

        private void classes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedClass = classes.SelectedItem as KeyValuePair<int, string>?;
            ExamB examB = new ExamB();
            examList = examB.filter(selectedClass.Value.Key);
            ex.ItemsSource = null;
            ex.ItemsSource = examList;
        }

        private void ResultPage_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Result());
        }
    }
}
