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
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Page
    {
        List<ResultB> results;   
        public Result()
        {
            InitializeComponent();
            results = new List<ResultB>();
            load_grid();
            loadClass();
        }

        private void EXAM(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Exam());
        }

        private void Entry_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new ResultEntry());
        }

        private void Mainframe_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void load_grid()
        {
            ResultB resultB = new ResultB();
            results = resultB.GetResults();

            ResultGrid.ItemsSource = results;
        }
        private void view_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button?.DataContext is ResultB person)
            {
                int id = person.ResultId;
                int classid = person.Exams.Classes.classId;
                Mainframe.Navigate(new ResultEntry(0,classid,id));
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
            if (button?.DataContext is ResultB person)
            {

                int id = person.ResultId;
                ResultB resultB = new ResultB();
                if (resultB.DeleteResult(id))
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

        private void loadClass()
        {
            ClassB classB = new ClassB();
            classes.ItemsSource = classB.load();
            classes.DisplayMemberPath = "Value";
            classes.SelectedValuePath = "Key";

        }
        private void classes_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var selectedClass = classes.SelectedItem as KeyValuePair<int, string>?;
            ResultB resultB = new ResultB();
            results = resultB.FilterResult(selectedClass.Value.Key);
            ResultGrid.ItemsSource = null;
            ResultGrid.ItemsSource = results;
        }
    }
}
