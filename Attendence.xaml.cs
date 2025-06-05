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
    /// Interaction logic for Attendence.xaml
    /// </summary>
    public partial class Attendence : Page
    {
        List<StudentB> students;
        List<AttendenceB> attenants;
        private int Batch_id {  get; set; }
        private int class_id { get; set; }
        private DateTime tday { get; set; }
        private bool isMarked {  get; set; }
        public Attendence()
        {
            InitializeComponent();
            isMarked = false;
            students = new List<StudentB>();
            attenants = new List<AttendenceB>();
            load();
            
        }

        private void P_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is StudentB person)
            {
                int id = person.id;
                AttendenceB attendenceB = new AttendenceB();
                int index = attenants.FindIndex(r => r.studentId == id);

                if (index == -1)
                {
                    attenants.Add(new AttendenceB
                    {
                        studentId = id,
                        batchId = Batch_id,
                        classId = class_id,
                        status = 'P',
                        Adate = tday,
                    });
                }
                else
                {
                    attenants = attendenceB.UpdateAttendanceIfExists(attenants, index, 'P');
                }
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void A_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is StudentB person)
            {
                int id = person.id;
                AttendenceB attendenceB = new AttendenceB();
                int index = attenants.FindIndex(r => r.studentId == id);

                if (index == -1)
                {
                    attenants.Add(new AttendenceB
                    {
                        studentId = id,
                        batchId = Batch_id,
                        classId = class_id,
                        status = 'A',
                        Adate = tday,
                    });
                }
                else
                {
                    attenants = attendenceB.UpdateAttendanceIfExists(attenants, index, 'A');
                }
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void L_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is StudentB person)
            {
                int id = person.id;
                AttendenceB attendenceB = new AttendenceB();
                int index = attenants.FindIndex(r => r.studentId == id);

                if (index == -1)
                {
                    attenants.Add(new AttendenceB
                    {
                        studentId = id,
                        batchId = Batch_id,
                        classId = class_id,
                        status = 'L',
                        Adate = tday,
                    });
                }
                else
                {
                    attenants = attendenceB.UpdateAttendanceIfExists(attenants, index, 'L');
                }
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void load_data(int batch, int cls)
        {
            students.Clear();
            StudentB studentB = new StudentB();
            students = studentB.attend(batch, cls);

            std.ItemsSource = null;
            std.ItemsSource = students;

        }
        private void load()
        {
            BatchesB batchesB = new BatchesB();
            batch.ItemsSource = batchesB.loadBatch();
            batch.DisplayMemberPath = "Value";
            batch.SelectedValuePath = "Key";


            ClassB classB = new ClassB();
            cal.ItemsSource = classB.load();
            cal.DisplayMemberPath = "Value";
            cal.SelectedValuePath = "Key";
        }

        private void cal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fil = cal.SelectedItem as KeyValuePair<int, string>?;
            class_id = fil.Value.Key;
            classes.Content = fil.Value.Value;
        }

        private void batch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fil = batch.SelectedItem as KeyValuePair<int, string>?;
            Batch_id = fil.Value.Key;
        }

        private void Atd_Click(object sender, RoutedEventArgs e)
        {
            if (!isMarked && date.SelectedDate != null && batch.SelectedValue != null && cal.SelectedValue != null)
            {
                batch.IsEnabled = false;
                cal.IsEnabled = false;
                date.IsEnabled = false;
                Atd.Content = "Save";
                load_data(Batch_id, class_id);
            }
            else if (isMarked)
            {
                batch.IsEnabled = true;
                date.IsEnabled = true;
                cal.IsEnabled = true;
                AttendenceB attendenceB = new AttendenceB();
                std.ItemsSource = null;
                if(attendenceB.addAttendence(attenants))
                {
                    MessageBox.Show("SuccessFull");
                    pre.Content = attenants.Count(a => a.status == 'P');
                    abs.Content = attenants.Count(a => a.status == 'A');
                    leave.Content = attenants.Count(a => a.status == 'L');
                }
            }

            else
            {
                MessageBox.Show("Please Select Class, Batch or Date");
            }
            isMarked = !isMarked;
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tday = Convert.ToDateTime(date.SelectedDate);
        }
    }
}
