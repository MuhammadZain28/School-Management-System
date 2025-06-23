using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;
using Microsoft.Data.Sqlite;

namespace LMS.DL
{
    class FeeD
    {
        public int TotalFee()
        {
            string query = $"Select sum(amount_paid) from fee_records;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int sum = 0;
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    sum = reader.GetInt32(0);
                }
                else
                {
                    sum = 0;
                }
            }
            reader.Close();
            return sum;
        }
        public int FilterTotalFee(int month)
        {
            string query = $"SELECT sum(amount_paid) FROM fee_records f WHERE strftime('%m', f.payment_date) = '{month:00}';";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int sum = 0;
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    sum = reader.GetInt32(0);
                }
                else
                {
                    sum = 0;
                }
            }
            reader.Close();
            return sum;
        }
        public List<FeeB> getFee()
        {
            List<FeeB> list = new List<FeeB>();
            try
            {
                string query = $@"SELECT f.fee_id, s.roll_no, s.name, f.amount_paid, f.status, f.due_amount, f.payment_date,s.student_id FROM fee_records f JOIN students s ON s.student_id = f.student_id  ORDER BY f.amount_paid DESC;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    list.Add(new FeeB
                    {
                        FeeId = reader.GetInt32(0),
                        roll = reader.GetString(1),
                        Name = reader.GetString(2),
                        AmountPaid = reader.GetInt32(3),
                        Status = reader.GetBoolean(4),
                        DueAmount = reader.GetDecimal(5),
                        PaymentDate = reader.GetDateTime(6),
                        stdId = reader.GetInt32(7),
                    });
                }
                reader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<FeeB> filter(int month)
        {
            List<FeeB> list = new List<FeeB>();
            try
            {
                string query = $@"
                                    SELECT f.fee_id, s.roll_no, s.name, f.amount_paid, f.status, f.due_amount, f.payment_date, s.student_id 
                                    FROM fee_records f 
                                    JOIN students s ON s.student_id = f.student_id 
                                    WHERE strftime('%m', f.payment_date) = '{month:00}' 
                                    ORDER BY f.amount_paid DESC;";

                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    list.Add(new FeeB
                    {
                        FeeId = reader.GetInt32(0),
                        roll = reader.GetString(1),
                        Name = reader.GetString(2),
                        AmountPaid = reader.GetInt32(3),
                        Status = reader.GetBoolean(4),
                        DueAmount = reader.GetDecimal(5),
                        PaymentDate = reader.GetDateTime(6),
                        stdId = reader.GetInt32(7),
                    });
                }
                reader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }
        public List<FeeB> graph()
        {
            List<FeeB> list = new List<FeeB>();
            try
            {
                string query = @"SELECT strftime('%m', payment_date) AS month, SUM(amount_paid) 
                 FROM fee_records 
                 GROUP BY strftime('%m', payment_date)";

                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    list.Add(new FeeB
                    {
                        AmountPaid = reader.GetInt32(0),
                        month = reader.GetInt32(1),
                    });
                }
                reader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public FeeB searchFee(int id)
        {
            FeeB fee = new FeeB();
            try
            {
                string query = $@"SELECT f.fee_id, s.roll_no, s.name, f.amount_paid, f.status, f.due_amount, f.payment_date,s.student_id FROM fee_records f JOIN students s ON s.student_id = f.student_id Where f.fee_id = {id};";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    fee = (new FeeB
                    {
                        FeeId = reader.GetInt32(0),
                        roll = reader.GetString(1),
                        Name = reader.GetString(2),
                        AmountPaid = reader.GetInt32(3),
                        Status = reader.GetBoolean(4),
                        DueAmount = reader.GetDecimal(5),
                        PaymentDate = reader.GetDateTime(6),
                        stdId = reader.GetInt32(7),
                    });
                }
                reader.Close();
                return fee;
            }
            catch
            {
                return fee;
            }
        }
        public bool AddRecord(FeeB fee, int id, int status)
        {
            try
            {
                string formattedDate = fee.PaymentDate.ToString("yyyy-MM-dd");
                string query = @$"
                                    UPDATE fee_records 
                                    SET amount_paid = {fee.AmountPaid}, status = {status}, payment_date = '{formattedDate}' 
                                    WHERE student_id = {id} 
                                      AND strftime('%m', payment_date) = strftime('%m', '{formattedDate}') 
                                      AND strftime('%Y', payment_date) = strftime('%Y', '{formattedDate}');
                                    ";

                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return false;
            }
            
        }
        public void startOfMonth()
        {
            try
            {
                string insertLog = $"INSERT INTO month_log (log_date) VALUES (DATE('now'));";
                DatabaseHelper.Instance.Update(insertLog);
                MessageBox.Show("Fee Required");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
        public bool Logged(DateTime mon)
        {
            int count = 0;
            try
            {
                string formattedDate = mon.ToString("yyyy-MM-dd");
                string query = $@"
                                SELECT COUNT(*) 
                                FROM month_log 
                                WHERE strftime('%m', log_date) = strftime('%m', '{formattedDate}') 
                                  AND strftime('%Y', log_date) = strftime('%Y', '{formattedDate}')";


                using (SqliteDataReader reader = DatabaseHelper.Instance.getData(query))
                {
                    if (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            return count == 0;
        }

        public int stdId(string roll)
        {
            int id = 0;
            string query = $"Select student_id from students where roll_no = '{roll}';";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            reader.Close();
            return id;
        }
        public bool deleteRecord(int id)
        {
            try
            {
                string query = @$"Delete from fee_records where student_id = {id};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetDataTable(int year)
        {
            DataTable dt = new DataTable();
            string query = $"select * from finance_report where Year = {year};";
            dt = DatabaseHelper.Instance.GetTable(query);
            return dt;
        }
        public DataTable GetDataTableMonTH(int month)
        {
            DataTable dt = new DataTable();
            string query = $@"SELECT name as `Name`, roll_no as `Roll No`, due_amount as `Due`, amount_paid as `Paid`, payment_date as `Payment Date` from students s
                            JOIN fee_records f on f.student_id = s.student_id where CAST(strftime('%m', `Payment Date`) as int ) = {month};";
            dt = DatabaseHelper.Instance.GetTable(query);
            return dt;
        }
    }
}
