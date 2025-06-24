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

namespace LMS
{
    /// <summary>
    /// Interaction logic for TeacherAttendence.xaml
    /// </summary>
    public partial class TeacherAttendence : Page
    {
        List<TeacherB> teachers;

        List<TeacherAttendenceB> attenants;
        private DateTime tday { get; set; }
        private bool isMarked { get; set; }
        private bool manualSelect { get; set; } = false;

        public TeacherAttendence()
        {
            InitializeComponent();
            NavigationState.timespan = DateTime.Now.ToString("hh:mm tt");
        }

        private void student_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Attendence());
        }
        private void P_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is TeacherB person)
            {
                int id = person.id;
                TeacherAttendenceB attendenceB = new TeacherAttendenceB();
                int index = attenants.FindIndex(r => r.teacher_id == id);
                if (manualSelect && !NavigationState.SelectSame)
                {
                    Time time1 = new Time();
                    time1.ShowDialog();
                }
                if (index == -1)
                {
                    attenants.Add(new TeacherAttendenceB
                    {
                        teacher = new TeacherB()
                        {
                            id = id,
                        },
                        status = "P",
                        date = tday.ToString("yyyy-MM-dd"),
                        checkIn = NavigationState.timespan,
                    });
                }
                else
                {
                    attenants = attendenceB.UpdateAttendanceIfExists(attenants, index, "P", NavigationState.timespan);
                }
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void A_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is TeacherB person)
            {
                int id = person.id;
                int index = attenants.FindIndex(r => r.teacher_id == id);

                TeacherAttendenceB attendenceB = new TeacherAttendenceB();
                if (index == -1)
                {
                    attenants.Add(new TeacherAttendenceB
                    {
                        teacher = new TeacherB()
                        {
                            id = id,
                        },
                        status = "A",
                        date = tday.ToString("yyyy-MM-dd"),
                        checkIn = DateTime.Now.ToString("HH:mm:ss"),
                    });
                }
                else
                {
                    attenants = attendenceB.UpdateAttendanceIfExists(attenants, index, "A");
                }
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void L_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is TeacherB person)
            {
                int id = person.id;
                TeacherAttendenceB attendenceB = new TeacherAttendenceB();
                int index = attenants.FindIndex(r => r.teacher_id == id);

                if (index == -1)
                {
                    attenants.Add(new TeacherAttendenceB
                    {
                        teacher = new TeacherB()
                        {
                            id = id,
                        },
                        status = "L",
                        date = tday.ToString("yyyy-MM-dd"),
                        checkIn = DateTime.Now.ToString("HH:mm:ss"),
                    });
                }
                else
                {
                    attenants = attendenceB.UpdateAttendanceIfExists(attenants, index, "L");
                }
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void load_data()
        {
            TeacherB teacherB = new TeacherB();
            teachers = teacherB.getData();

            std.ItemsSource = null;
            std.ItemsSource = teachers;

            attenants = AttendenceD.getTeacherData(tday.ToString("yyyy-MM-dd"));
            foreach (var item in attenants)
            {
                MessageBox.Show(item.teacher_id + " " + item.checkOut);
            }
        }

        private void Atd_Click(object sender, RoutedEventArgs e)
        {
            if (!isMarked && date.SelectedDate != null)
            {
                date.IsEnabled = false;
                Atd.Content = "💾   SAVE";
                load_data();
                isMarked = true;
                NavigationState.SelectSame = false;
                NavigationState.HasUnsavedChanges = true;
            }
            else if (isMarked)
            {
                date.IsEnabled = true;
                TeacherAttendenceB attendenceB = new TeacherAttendenceB();
                std.ItemsSource = null;
                if (attendenceB.addData(attenants))
                {
                    MessageBox.Show("SuccessFul");
                    pre.Content = attenants.Count(a => a.status == "P");
                    abs.Content = attenants.Count(a => a.status == "A");
                    leave.Content = attenants.Count(a => a.status == "L");
                    Atd.Content = "➕   ADD NEW";
                }
                isMarked = false;
                NavigationState.HasUnsavedChanges = false;
            }

            else
            {
                MessageBox.Show("Please Select Class, Batch or Date");
            }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tday = Convert.ToDateTime(date.SelectedDate);
        }

        private void teacher_attendence_click(object sender, RoutedEventArgs e)
        {
            if (isMarked)
            {
                MessageBoxResult result = MessageBox.Show("Do You want to save data?", "Unsaved Work", MessageBoxButton.YesNo, icon: MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    TeacherAttendenceB attendenceB = new TeacherAttendenceB();
                    attendenceB.addData(attenants);
                    NavigationState.HasUnsavedChanges = false;
                }
            }
            MainFrame.Navigate(new TeacherAttendence());
        }

        private void out_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (manualSelect && !NavigationState.SelectSame)
            {
                Time time1 = new Time();
                time1.ShowDialog();
            }
            if (button?.DataContext is TeacherB person)
            {
                int Id = person.id;

                TeacherAttendenceB attendenceB = new TeacherAttendenceB();

                int index = attenants.FindIndex(r => r.teacher_id == Id);

                MessageBox.Show("Index " + index);

                attendenceB.Out(attenants, index, NavigationState.timespan);

                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            manualSelect = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NavigationState.timespan = DateTime.Now.ToString("hh:mm tt");
            manualSelect = false;
        }
    }
}
