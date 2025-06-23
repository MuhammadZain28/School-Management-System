using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    class BatchesD
    {
        public bool addBatch(string batchName)
        {
            try
            {
                string query = $"INSERT INTO Branch (Branch_name) VALUES ('{batchName}')";
                int rows = DatabaseHelper.Instance.Update(query);

                MessageBox.Show("Batch added successfully.");
                return true;
            }
            catch ( Exception e ) 
            {
                MessageBox.Show("Error :" + e);
                return false;
            }
        }

        public Dictionary<int, string> loadComboBoxBatch()
        {
            Dictionary<int, string> batches = new Dictionary<int, string>();
            try
            {
                string query = "SELECT Branch_id, Branch_name FROM Branch;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    batches.Add(
                        reader.GetInt32(0), 
                        reader.GetString(1));
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return batches;
        }
    }
}
