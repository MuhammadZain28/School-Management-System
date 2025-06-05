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
    /// Interaction logic for AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Page
    {
        public AddEvent()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            EventB eventB = InputData();
            if (eventB.AddEvent(eventB))
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Unsuccessful");
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Event());
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private EventB InputData()
        {
            EventB eventB = new EventB();   
            eventB.name = Name.Text;
            TextRange textRange = new TextRange(description.Document.ContentStart, description.Document.ContentEnd);
            eventB.description = textRange.Text;
            eventB.Date = (DateTime)date.SelectedDate;
            return eventB;
        }
    }
}
