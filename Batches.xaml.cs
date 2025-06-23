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
        private bool IsSaved = true;
        public Batches()
        {
            InitializeComponent();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }
        private void AddData()
        {
            string Name = name.Text;
            
            BatchesB batchesB = new BatchesB();
            if (batchesB.addBatch(Name))
            {
                name.Text = "";
                IsSaved = true;
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.No;
            if (!string.IsNullOrWhiteSpace(name.Text))
            {
                IsSaved = false; 
            }
            if (!IsSaved)
            {
                result = MessageBox.Show("Want to save data", "Unsaved Work", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    AddData();
                }
            }
            MainFrame.Navigate(new DashBoard());
        }
    }
}
