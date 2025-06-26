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
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        private bool isPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;
        public SignUp()
        {
            InitializeComponent();
            load();
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
                toggleButton.Content = "👁"; // Change icon to "view"
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
        private void ConfirmToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;

            if (isConfirmPasswordVisible)
            {
                ConfirmvisibleBox.Text = ConfirmPasswordBox.Password;
                ConfirmvisibleBox.Visibility = Visibility.Visible;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                ConfirmtoggleButton.Content = "🙈"; 
            }
            else
            {
                ConfirmPasswordBox.Password = ConfirmvisibleBox.Text;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                ConfirmvisibleBox.Visibility = Visibility.Collapsed;
                ConfirmtoggleButton.Content = "👁"; 
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isConfirmPasswordVisible)
                ConfirmvisibleBox.Text = ConfirmPasswordBox.Password;
        }

        private void ConfirmVisibleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isConfirmPasswordVisible)
                ConfirmPasswordBox.Password = ConfirmvisibleBox.Text;
        }

        private void sign_in_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Login());
        }

        private void sign_up_Click(object sender, RoutedEventArgs e)
        {
            if(isConfirmPasswordVisible)
            {
                ConfirmPasswordBox.Password = ConfirmvisibleBox.Text;
            }
            if (isPasswordVisible)
            {
                PasswordBox.Password = visibleBox.Text;
            }
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            string otp = NotificationD.GenerateOtp();
            if (NotificationD.SendOTP(Email.Text, otp))
            {
                MainFrame.Navigate(new OTP(otp, Email.Text, PasswordBox.Password, Phone.Text));
            }
        }
        private void load()
        {
            Phone.Items.Add("admin");
            Phone.Items.Add("coordinator");
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
