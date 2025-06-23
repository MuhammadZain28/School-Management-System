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

namespace LMS
{
    /// <summary>
    /// Interaction logic for ProjectReport.xaml
    /// </summary>
    public partial class ProjectReport : Page
    {
        public ProjectReport()
        {
            InitializeComponent();
        }

        private void attend_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AttendenceReport(0));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AttendenceReport(1));
        }

        private void fee_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AttendenceReport(2));
        }

        private void teacher_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate (new AttendenceReport(3));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void attendence_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AttendenceReport(4));
        }

        private void salary_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AttendenceReport(5));
        }
    }
}
