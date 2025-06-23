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
using LMS.DL;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public bool IsExpanded { get; set; }
        public Admin()
        {
            InitializeComponent();
            MainFrame.Navigating += MainFrame_Navigating;
            MainFrame.Navigate(new DashBoard());
        }
        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (NavigationState.HasUnsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes. Do you want to discard them?",
                                             "Unsaved Work", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true; // Cancel navigation
                    return;
                }

                NavigationState.HasUnsavedChanges = false; // Reset flag if continuing
            }
        }
        private void Menu(object sender, RoutedEventArgs e)
        {
            if (!IsExpanded)
            {
                var storyboard = (Storyboard)FindResource("ExpandBorderStoryboard");
                storyboard.Begin();

            }
            else
            {
                var storyboard = (Storyboard)FindResource("UnExpandBorderStoryboard");
                storyboard.Begin();
            }
            IsExpanded = !IsExpanded;
            SideMenu.Content = IsExpanded ? "⏴" : "⏵";
        }

        private void Finance(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Finance());
        }

        private void std_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Student());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void Dashboard(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashBoard());
        }

        private void Attendence(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Attendence());
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            LogFrame.Navigate(new Login());
        }

        private void Course(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Course());
        }

        private void Exam(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Exam());
        }

        private void Event(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Event());
        }

        private void Teacher(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Teacher());
        }

        private void report_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProjectReport());
        }

        private void LogFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
