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
using LMS.BL;
using LMS.DL;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : Page
    {
        private List<StudentB> students;
        private bool IsSearch {  get; set; }
        public Student()
        {
            InitializeComponent();
            students = new List<StudentB>();
            IsSearch = false;
            load_data();  // Load Data in Table
            load();  // Load Data in Combo Box
        }

        private void Menu(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DashBoard(object sender, RoutedEventArgs e)
        {
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void load()
        {
            ClassB classB = new ClassB();
            filter.ItemsSource = classB.load();
            filter.DisplayMemberPath = "Value";
            filter.SelectedValuePath = "Key";
        }
        private void Admission_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StudentInput());
        }
        private void load_data()
        {
            students.Clear();
            StudentB studentB = new StudentB();
            students = studentB.getData();

            std.ItemsSource = null;
            std.ItemsSource = students;
            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is StudentB person)
            {
                int Id = person.id;
                MainFrame.Navigate(new StudentInput(Id, true));
            }
        }
        private void view_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is StudentB person)
            {
                int Id = person.id;
                MainFrame.Navigate(new StudentInput(Id));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is StudentB person)
            {
                int Id = person.id;
                MessageBox.Show("" + Id);
                if (person.deleteData(Id))
                {
                    MessageBox.Show("Deleted SuccessFully");

                    load_data();
                }
                else
                {
                    MessageBox.Show("UnsuccessFul");
                }
            }
        }

        private void searchStd()
        {
            List<StudentB> studentbs = new List<StudentB>();
            string nm = search.Text;
            studentbs = students.FindAll(t => t.name.Length >= 3 && t.name.Substring(0, 3) == nm.Substring(0, 3));
            if (!IsSearch)
            {
                std.ItemsSource = null;
                std.ItemsSource = studentbs;
            }
            else
            {
                std.ItemsSource = null;
                std.ItemsSource = students;
            }
            IsSearch = !IsSearch;
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            searchStd();    
        }

        private void filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fil = filter.SelectedItem as KeyValuePair<int, string>?;
            StudentB studentB = new StudentB();
            if (fil.Value.Key == -1)
            {
                load_data();
                return;
            }
            std.ItemsSource = null;
            std.ItemsSource = studentB.filterData(fil.Value.Key);
        }
    }
}
