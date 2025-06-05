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
using Microsoft.VisualBasic.ApplicationServices;

namespace LMS
{
    /// <summary>
    /// Interaction logic for coAnnouncements.xaml
    /// </summary>
    public partial class coAnnouncements : Page
    {
        private Strategy _strategy;
        public coAnnouncements()
        {
            InitializeComponent();
            _strategy = new Strategy(new NotificationD());
            load();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashBoard(UserD.currentUser.role));
        }
        private void load()
        {
            AnnouncementB announcementBs = new AnnouncementB();

            ann_grid.ItemsSource = announcementBs.GetAnnouncements();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            InAppNotifications notifications = new InAppNotifications();
            notifications.UserId = UserD.currentUser.id;
            TextRange textRange = new TextRange(message.Document.ContentStart, message.Document.ContentEnd);
            notifications.message = textRange.Text;

            if (_strategy.Send(notifications))
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Unsuccessful");
            }
        }
    }
}
