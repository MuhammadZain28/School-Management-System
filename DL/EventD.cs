using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    class EventD
    {
        private static CourseD? instance = null;


        public static CourseD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CourseD();
                }
                return instance;
            }
        }
        public static List<EventB> GetEvent()
        {
            List<EventB> events = new List<EventB>();
            string query = $"Select event_id,event_name, event_date,description from events order by event_date";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                events.Add(new EventB
                {
                    event_id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    Date = reader.GetDateTime(2),
                    description = reader.GetString(3),
                });
            }
            reader.Close();
            return events;
        }
        public static EventB GetEventByMonth(DateTime date)
        {
            EventB events = new EventB();
            string query = $"Select event_id,event_name, event_date,description from events where event_date = '{date.ToString("yyyy-MM-dd")}'";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                events = new EventB
                {
                    event_id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    Date = reader.GetDateTime(2),
                    description = reader.GetString(3),
                };
            }
            reader.Close();
            return events;
        }
        public static bool DeleteEvent(int eventId)
        {
            try
            {
                string query = $"DELETE FROM events WHERE event_id = {eventId}";
                DatabaseHelper.Instance.Update(query); 
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting event: " + ex.Message);
                return false;
            }

        }
        public static bool InsertEvent(EventB eventB)
        {
            try
            { 
                string query = $"INSERT INTO events (event_name, event_date, description) VALUES ('{eventB.name}', '{eventB.Date.ToString("yyyy-MM-dd")}', '{eventB.description}')";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting event: " + ex.Message);
                return false;
            }
        }

    }
}
