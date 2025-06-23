using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace LMS.DL
{
    class NotificationD : INotificationStrategy
    {

        private static NotificationD? instance = null;


        public static NotificationD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotificationD();
                }
                return instance;
            }
        }
        public NotificationD() { }

        public bool Send(InAppNotifications notifications)
        {
            try
            {
                string query = $"INSERT INTO notifications (user_id, message, seen_status, date) VALUES ({notifications.UserId}, '{notifications.message}', 0, DATE('now'));";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public int Count()
        {
            int count = 0;
            try
            {
                string query = $"SELECT count(notif_id) FROM notifications n WHERE seen_status = 0;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return count;
            }
        }
        public List<InAppNotifications> GetNotifications()
        {
            List<InAppNotifications> notifications = new List<InAppNotifications>();
            string query = $"SELECT notif_id, message, username FROM notifications n natural join logins l WHERE seen_status = 0 ORDER BY date DESC;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                notifications.Add(new InAppNotifications()
                {
                    Notif_id = reader.GetInt32(0),
                    message = reader.GetString(1),
                    UserName = reader.GetString(2),

                });
            }
            return notifications;
        }
        public List<InAppNotifications> GetReadNotifications()
        {
            List<InAppNotifications> notifications = new List<InAppNotifications>();
            string query = $"SELECT notif_id, message, username, date FROM notifications n natural join logins l WHERE strftime('%Y-W%W', date) = strftime('%Y-W%W', 'now') and seen_status = 1 ORDER BY seen_status desc, date DESC;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                notifications.Add(new InAppNotifications()
                {
                    Notif_id = reader.GetInt32(0),
                    message = reader.GetString(1),
                    UserName = reader.GetString(2),
                    Date = reader.GetDateTime(3).ToString("dd-MM-yyyy"),

                });
            }
            return notifications;
        }

        public bool MarkAsRead(int userId)
        {
            try
            {
                string query = $"UPDATE notifications SET seen_status = 1 WHERE notif_id = {userId};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public static string GenerateOtp()
        {
            Random random = new Random();
            string otp = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 10);
            }
            return otp;
        }
        public static bool SendOTP(string toEmail, string otp)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("zaina.azhar2005@gmail.com", "sikjbephunoliqsa");

                MailMessage message = new MailMessage();
                message.From = new MailAddress("zaina.azhar2005@gmail.com");
                message.To.Add(toEmail);
                message.Subject = "Your OTP Code";
                message.Body = $"Your OTP is: {otp}";
                smtp.Send(message);

                MessageBox.Show("OTP sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending OTP: " + ex.Message);
                return false;
            }
        }
        public static void SendClientEmail(string mail)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("zaina.azhar2005@gmail.com", "sikjbephunoliqsa"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("zaina.azhar2005@gmail.com"),
                    Subject = "Test from Client",
                    Body = "This email was sent from the client computer!",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);

                MessageBox.Show("Email sent successfully from client!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send: " + ex.Message);
            }
        }

    }
}
