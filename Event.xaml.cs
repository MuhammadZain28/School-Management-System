using System;
using System.Collections.Generic;
using System.Globalization;
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
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Event.xaml
    /// </summary>
    public partial class Event : Page
    {
        private List<EventB> events;
        public Event()
        {
            InitializeComponent();
            events = new List<EventB>();
            loadGrid(); 
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new AddEvent());
        }

        private void Mainframe_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void loadGrid()
        {
            EventB eventB = new EventB();
            events = eventB.getData();
            SheduleGrid.ItemsSource = events;
            if (events.Count > 0)
            {
                upcoming.Content = events[0].name;
                if (events[0].Date > DateTime.Now)
                {
                    TimeSpan remaining = events[0].Date - DateTime.Now;
                    time.Content = $"{remaining.Days} Days, {remaining.Hours} Hours, {remaining.Minutes} Mins";
                }
                else
                {
                    time.Content = "Event Finished";
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is EventB person)
            {

                int id = person.event_id;
                if (person.DeleteData(id))
                {
                    MessageBox.Show("Success");
                    loadGrid();  
                }
                else
                {
                    MessageBox.Show("Unsuccessful");
                }


            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar.SelectedDate;
            eve.Content = "Event : ";
            tim.Content = "Time : ";
            if (selectedDate.HasValue)
            {
                DateTime date = selectedDate.Value;
                EventB eventB = new EventB();
                eventB = eventB.GetEvent(date);
                eve.Content += eventB.name;
                tim.Content += eventB.Date.ToString("dd-MM-yyyy");
            }
            else
            {
                MessageBox.Show("No date selected.");
            }

        }
    }
}
