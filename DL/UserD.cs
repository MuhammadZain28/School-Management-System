using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    class UserD
    {

        private static UserD? instance = null;


        public static UserD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserD();
                }
                return instance;
            }
        }
        public static UserB currentUser { get; private set; }
        public static bool signIn(string username, string password)
        {
            try
            {
                string query;
                query = $"select count(*), role from logins where username = '{username}' and password = '{password}'";

                int count = Convert.ToInt32(DatabaseHelper.Instance.ExecuteScalar(query));
                return (count > 0);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                return false;
            }
        }
        public static bool ChecksAdmin()
        {
            try
            {
                string query;
                query = $"select count(*) from logins where role = 'admin'";

                int count = Convert.ToInt32(DatabaseHelper.Instance.ExecuteScalar(query));
                return (count > 0);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                return false;
            }
        }

        public static UserB GetRole(string username, string password)
        {
            try
            {
                string query;
                query = $"select login_id, role from logins where username = '{username}' and password = '{password}'";

                
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                {
                    currentUser = new UserB()
                    {
                        id = reader.GetInt32(0),
                        role = reader.GetString(1),
                    };
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error on get Role: " + ex.Message);
            }
            return currentUser;
        }

        public static bool AddUser(string email, string pass, string role = "coordinator")
        {
            if (true)
            {
                try
                {
                    string query;
                    query = $"INSERT INTO logins (username,password,role)  VALUES ('{email}' , '{pass}' , '{role}')";

                    DatabaseHelper.Instance.Update(query);
                    return true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while adding the User: " + ex.Message);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Admin Already Exist is System");
                return false;
            }
        }
    }
}
