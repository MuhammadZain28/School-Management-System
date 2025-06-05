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
    /// Interaction logic for Hire.xaml
    /// </summary>
    public partial class Hire : Page
    {
        private List<QualificationB> qualificationBs;
        private int id {  get; set; } 
        private int count { get; set; }
        public Hire(int ID = 0)
        {
            InitializeComponent();
            qualificationBs = new List<QualificationB>();
            Submit.IsEnabled = false;
            LoadYear();
            id = ID;
            if (id > 0)
            {
                view_teacher();
            }
        }
        private void view_teacher()
        {
            TeacherB teacher = new TeacherB();
            teacher = teacher.SearchData(id);

            name.Text = teacher.name;
            Contact.Text = teacher.contact;
            salary.Text = Convert.ToString(teacher.salary); 
            Hiring.SelectedDate = DateTime.ParseExact(teacher.Joining, "dd-MM-yyyy", null);
            Degree.Text = teacher.Qualifications;
            Institute.Text = teacher.Institutes;
            Year.SelectedItem = teacher.year;

            name.IsEnabled = false;
            Hiring.IsEnabled = false;
            salary.IsEnabled = false;
            Contact.IsEnabled = false;
            Degree.IsEnabled = false;
            Year.IsEnabled = false;
            Institute.IsEnabled = false; 
            more.IsEnabled = false;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (id == 0)
            {
                TeacherB teacherB = new TeacherB();
                teacherB.name = name.Text;
                teacherB.contact = Contact.Text;
                teacherB.salary = Convert.ToInt32(salary.Text);
                teacherB.Joining = Hiring.Text;
                if (teacherB.addData(teacherB, qualificationBs))
                {
                    MessageBox.Show("Teacher Added Successfully");
                    qualificationBs.Clear();
                }
                else
                {
                    MessageBox.Show("UnsuccessFul Try Again Later");
                }
            }
            else if (count == 0)
            {
                TeacherB teacherB = new TeacherB();
                teacherB.contact = Contact.Text;
                teacherB.salary = Convert.ToDecimal(salary.Text);
                if (teacherB.UpdateData(teacherB, id))
                {
                    MessageBox.Show("Data updated Successfully");
                    qualificationBs.Clear();
                }
                else
                {
                    MessageBox.Show("UnsuccessFul Update Later");
                }
            }
            else
            {
                QualificationB qualification = new QualificationB();
                if (qualification.AddData(qualificationBs, id))
                {
                    MessageBox.Show("Data updated Successfully");
                    qualificationBs.Clear();
                }
                else
                {
                    MessageBox.Show("UnsuccessFul Update Later");
                }
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Teacher());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void LoadYear()
        {
            for (int year = 1960; year <= DateTime.Now.Year; year++)
            {
                Year.Items.Add(year.ToString());
            }
        }

        private void more_Click(object sender, RoutedEventArgs e)
        {
                qualificationBs.Add(new QualificationB()
                {
                    degree = Degree.Text,
                    institute = Institute.Text,
                    year = Convert.ToInt32(Year.SelectedItem)
                });
                name.IsEnabled = false;
                Contact.IsEnabled = false;
                salary.IsEnabled = false;
                Hiring.IsEnabled = false;
                Degree.Text = "";
                Institute.Text = "";
                Degree.IsEnabled = true;
                Institute.IsEnabled = true;
                Year.IsEnabled = true;
                Submit.IsEnabled = true;
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            count = 0;
            Contact.IsEnabled = true;
            salary.IsEnabled = true;
            Submit.IsEnabled = true;
        }

        private void qual_Click(object sender, RoutedEventArgs e)
        {
            count = 1;
            Degree.IsEnabled = true;
            Institute.IsEnabled = true;
            Degree.Text = "";
            Institute.Text = "";
            Year.IsEnabled = true;
            more.IsEnabled = true;
            Submit.IsEnabled = true;
        }
    }
}
