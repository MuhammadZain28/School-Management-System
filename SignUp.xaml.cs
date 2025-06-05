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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        private bool isPasswordVisible = false;
        private bool isPasswordVisible1 = false;
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
                visibleBox.Text = ConfirmPasswordBox.Password;
                visibleBox.Visibility = Visibility.Visible;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                toggleButton.Content = "🙈"; 
            }
            else
            {
                ConfirmPasswordBox.Password = visibleBox.Text;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                visibleBox.Visibility = Visibility.Collapsed;
                toggleButton.Content = "👁"; // Change icon to "view"
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
                visibleBox.Text = ConfirmPasswordBox.Password;
        }

        private void VisibleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isPasswordVisible)
                ConfirmPasswordBox.Password = visibleBox.Text;
        }
        private void ToggleButton1_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible1 = !isPasswordVisible1;

            if (isPasswordVisible1)
            {
                visibleBox1.Text = PasswordBox1.Password;
                visibleBox1.Visibility = Visibility.Visible;
                PasswordBox1.Visibility = Visibility.Collapsed;
                toggleButton1.Content = "🙈"; 
            }
            else
            {
                PasswordBox1.Password = visibleBox1.Text;
                PasswordBox1.Visibility = Visibility.Visible;
                visibleBox1.Visibility = Visibility.Collapsed;
                toggleButton1.Content = "👁"; 
            }
        }

        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible1)
                visibleBox1.Text = PasswordBox1.Password;
        }

        private void VisibleBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isPasswordVisible1)
                PasswordBox1.Password = visibleBox1.Text;
        }

        private void sign_in_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Login());
        }

        private void sign_up_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            UserB userB = new UserB(Email.Text, PasswordBox1.Password, Phone.Text);

            bool result = userB.AddUser();

            try
            {
                if (result)
                    MainFrame.Navigate(new Login());
            }
            catch(Exception ex) {
                MessageBox.Show("Error : " +  ex.Message);
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
