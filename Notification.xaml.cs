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
using ControlzEx.Standard;
using LMS.BL;
using LMS.DL;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Page
    {
        private Strategy _strategy;
        public Notification()
        {
            InitializeComponent();
            load();
            _strategy = new Strategy(new NotificationD());
        }
        private void load()
        {
            Strategy _strategy = new Strategy(new NotificationD());
            List<InAppNotifications> notifications = _strategy.GetNotifications();
            
            std.ItemsSource = notifications;

            List<InAppNotifications> Readnotifications = _strategy.GetReadNotifications();

            read.ItemsSource = Readnotifications;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mark_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is InAppNotifications person)
            {
                int Id = person.Notif_id;
                if (_strategy.MarkAsRead(Id))
                {
                    MessageBox.Show("Readed");
                    button.Opacity = 50;
                }
                button.Background = new SolidColorBrush(Colors.FloralWhite);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
