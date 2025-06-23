using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;
using MySql.Data.MySqlClient;
using WinFormsApp1;

namespace LMS.BL
{
    public class AnnouncementB
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string announcement_for { get; set; }
        public string date { get; set; }
        public AnnouncementB() { }
        public bool addAnnouncement(string title, string message, string _for)
        {
            AnnouncementD announcementD = new AnnouncementD();
            string[] allowed = { "Students", "Teachers", "All" };
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(_for))
            {
                MessageBox.Show("All fields are required.");
                return false;
            }

            if (title.Length > 100)
            {
                MessageBox.Show("Title should not exceed 100 characters.");
                return false;
            }

            if (message.Length < 15)
            {
                MessageBox.Show("Message must be at least 15 characters long.");
                return false;
            }

            if (!allowed.Contains(_for))
            {
                MessageBox.Show("Invalid value for 'Announcement_For' field.");
                return false;
            }

            if (AnnouncementD.AddAnnouncement(title, message, currentDate, _for))
            {
                MessageBox.Show("Announcement added successfully.");
                return true;
            }
            return false;
        }
        public List<AnnouncementB> GetAnnouncements()
        {
            return AnnouncementD.GetAnnouncements();
        }
    }
}
