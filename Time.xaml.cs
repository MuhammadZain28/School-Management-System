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
using System.Windows.Shapes;
using LMS.DL;
using Microsoft.Xaml.Behaviors.Core;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Time.xaml
    /// </summary>
    public partial class Time : Window
    {
        public Time()
        {
            InitializeComponent();
            time.Items.Add("AM");
            time.Items.Add("PM");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Same Time is Applied for All !", "Alert", MessageBoxButton.YesNo, icon: MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                NavigationState.SelectSame = true;
            }
            else
            {
                NavigationState.SelectSame = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationState.timespan = hour.Text + ":" + min.Text + " " + time.Text;
            this.Close();
        }
    }
}
