using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for Announcement.xaml
    /// </summary>
    public partial class Announcement : Page
    {
        public Announcement()
        {
            InitializeComponent();
            load();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string selectedFor = announcementFor.Text; TextRange textRange = new TextRange(message.Document.ContentStart, message.Document.ContentEnd);
            string txtmessage = textRange.Text.Trim();
            string title = Title.Text;
            AnnouncementB announcement = new AnnouncementB();
            if(announcement.addAnnouncement(title, txtmessage, selectedFor))
            {
                MessageBox.Show("Announcement Created");
            }
            else
                MessageBox.Show("Failed");

        }
        private void load()
        {
            announcementFor.Items.Add("Teachers");
            announcementFor.Items.Add("Students");
            announcementFor.Items.Add("All");
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashBoard());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
                
        }
    }
}
