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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LMS
{
    /// <summary>
    /// Interaction logic for OTP.xaml
    /// </summary>
    public partial class OTP : Page
    {
        public string email {  get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string otp {  get; set; }
        public OTP(string otp, string email, string password, string role)
        {
            InitializeComponent();
            this.role = role;
            this.email = email;
            this.password = password;
            this.otp = otp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Input.Text == otp)
            {

                UserB userB = new UserB(email, password, role);

                bool result = userB.AddUser();

                try
                {
                    if (result)
                        MainFrame.Navigate(new Login());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Otp is Wrong: " + otp + "Input: "+ Input.Text);
            }
        }
    }
}
