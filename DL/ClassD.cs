using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using WinFormsApp1;
using LMS.BL;
using MessageBox = System.Windows.MessageBox;
using Microsoft.Data.Sqlite;

namespace LMS.DL
{
    internal class ClassD
    {
        public Dictionary<int, string> loadComboBoxClass()
        {
            Dictionary<int, string> batches = new Dictionary<int, string>();
            try
            {
                string query = "SELECT class_id, class_name FROM \"classes\" order by class_name;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    batches.Add(reader.GetInt32(0), reader.GetString(1));
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return batches;
        }
        public bool Insert(ClassB classB)
        {
            try
            {
                string query = $"INSERT into \"classes\"(class_name) Values ('{classB.name}')";
                DatabaseHelper.Instance.Update(query);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;

            }
        }

        public bool Delete(int id)
        {
            try
            {
                string query = $"DELETE from classes WHERE class_id = {id}";
                DatabaseHelper.Instance.Update(query);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;

            }
        }
    }
}
