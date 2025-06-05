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
    /// <summary>
    /// Interaction logic for Batches.xaml
    /// </summary>
    public partial class Batches : Page
    {
        public Batches()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string Name = name.Text;
            int Fee = Convert.ToInt32(fee.Text);
            string stDate = DateTime.Now.ToString("yyyy-MM-dd");
            string endDate = DateTime.Now.ToString("yyyy-MM-dd");
            CourseB courseB = new CourseB();
            //int id = ComboBox.SelectedIndex + 1;
            //courseB.setCourseID(id)

            BatchesB batchesB = new BatchesB(Name, stDate, endDate, Fee, courseB);
            batchesB.addBatch();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashBoard());
        }
    }
}
