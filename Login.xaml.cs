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

namespace LMS
{
    public partial class Login : Page
    {
        private bool isPasswordVisible = false;
        public Login()
        {
            InitializeComponent();
        }

        private void sign_in_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                PasswordBox.Password = visibleBox.Text;
            }
            UserB userB = new UserB(Email.Text, PasswordBox.Password);
           
            if (userB.signInFunc()) 
            {
                userB = userB.getRole(Email.Text, PasswordBox.Password);
                if (userB.role == "coordinator")
                {
                    MainFrame.Navigate(new Coordinator());
                }
                else
                    MainFrame.Navigate(new Admin());
            }
        }

        private void sign_up_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate (new SignUp());
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                visibleBox.Text = PasswordBox.Password;
                visibleBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                toggleButton.Content = "🙈";
            }
            else
            {
                PasswordBox.Password = visibleBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                visibleBox.Visibility = Visibility.Collapsed;
                toggleButton.Content = "👁"; 
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
                visibleBox.Text = PasswordBox.Password;
        }

        private void VisibleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isPasswordVisible)
                PasswordBox.Password = visibleBox.Text;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
