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
    class BatchesD
    {
        private static BatchesD? instance = null;
        public static BatchesD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BatchesD();
                }
                return instance;
            }
        }
        public static bool addBatch(string batchName)
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

        public static Dictionary<int, string> loadComboBoxBatch()
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
        public static List<BatchesB> BranchSalaries()
        {
            List<BatchesB> batches = new List<BatchesB>();
            try
            {
                string query = "SELECT Branch_id, Branch_name, sum(salary) FROM Branch NATURAL join Teachers natural join salary GROUP by Branch_id;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    batches.Add(new BatchesB
                    {
                        branchId = reader.GetInt32(0),
                        BranchName = reader.GetString(1),   
                        Salaries = reader.GetDecimal(2)
                    });
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return batches;
        }

        public static DataTable BranchSalariesTable()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT branch_id, branch_name FROM Branch NATURAL join Teachers natural join salary GROUP by branch_id;";

                dt = DatabaseHelper.Instance.GetTable(query);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return dt;
        }
    }
}
