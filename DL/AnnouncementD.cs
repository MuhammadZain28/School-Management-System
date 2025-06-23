using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    public class AnnouncementD
    {

        private static AnnouncementD? instance = null;


        public static AnnouncementD Instance()
        {
            if (instance == null)
            {
                instance = new AnnouncementD();
            }
            return instance;
        }
        public static bool AddAnnouncement(string title, string message, string date, string _for)
        {

            try
            {
                using (var connection = DatabaseHelper.Instance.getConnection())
                {
                    string query = $"SELECT count(*) FROM announcements WHERE title = '{title}' and announcement_date = '{date}'";

                    int count = Convert.ToInt32(DatabaseHelper.Instance.ExecuteScalar(query));
                    if (count > 0)
                    {
                        MessageBox.Show("An announcement with this title already exists.");
                        return false;
                    }

                    query = $"INSERT INTO announcements (title, message, announcement_date, announcement_for) VALUES ('{title}', '{message}', '{date}', '{_for}')";
                    DatabaseHelper.Instance.Update(query);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the announcement: " + ex.Message);
                return false;
            }
        }

        public static List<AnnouncementB> GetAnnouncements()
        {
            List<AnnouncementB> announcements = new List<AnnouncementB>();

            try
            {
                string query = "SELECT announcement_id, title, message, announcement_for, announcement_date FROM announcements;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    AnnouncementB ann = new AnnouncementB
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        description = reader.GetString(2),
                        announcement_for = reader.GetString(3),
                        date = reader.GetDateTime(4).ToString("dd-MM-yyyy"),
                    };

                    announcements.Add(ann);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return announcements;
        }
    }
}
