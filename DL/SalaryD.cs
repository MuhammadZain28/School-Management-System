using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;
using Microsoft.Data.Sqlite;
using WinFormsApp1;

namespace LMS.DL
{
    internal class SalaryD
    {
        private static SalaryD? instance = null;
        public static int total_days = 30;
        public static int non_working_days = 0;
        public static SalaryD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SalaryD();
                    
                }
                return instance;
            }
        }
        public static int NonWorkDays()
        {
            int days = 0;
            string query = $"select count(attendence_id) from teacher_attendance where strftime('%m', date) = {DateTime.Now.ToString("MM")} and status = '--'";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                total_days = reader.GetInt32(0);
            }
            reader.Close();
            return days;
        }
        public static List<SalaryB> getData()
        {
            List<SalaryB> salaries = new List<SalaryB>();
            try
            {
                string query = @$"SELECT SUM(CASE WHEN status = 'P' THEN 1 ELSE 0 END) AS PresentDays,
                                         SUM(CASE WHEN status = 'A' THEN 1 ELSE 0 END) AS AbsentDays,
                                         SUM(CASE WHEN status = 'L' THEN 1 ELSE 0 END) AS LeaveDays,
                                         t.teacher_id, max(salary), s.salary_id
                                  FROM teacher_attendence ta join teachers t on t.teacher_id = ta.teacher_id
                                  LEFT JOIN salary s on t.teacher_id = s.teacher_id
                                  WHERE strftime('%m', date) = strftime('%m', 'now') GROUP BY t.teacher_id;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                while (reader.Read())
                {
                    salaries.Add(new SalaryB
                    {
                        total_present = reader.GetInt32(0),
                        total_absent = reader.GetInt32(1),
                        total_Leave = reader.GetInt32(2),
                        teacher = new TeacherB
                        {
                            id = reader.GetInt32(3),
                            salary = reader.GetInt32(4),
                        },
                        salary_id = reader.IsDBNull(5)? 0 : reader.GetInt32(5),
                    });
                }
                reader.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
            return salaries;
        }
        public static bool Insert(SalaryB salary)
        {
            try
            {
                if (!isExist(salary.teacher_id, salary.date_paid))
                {
                    string query = $@"INSERT INTO salary (teacher_id, net_salary, bonus, deduction, date_paid, paid)
                                  VALUES ({salary.teacher.id}, {salary.net_salary}, 0, {salary.deduction}, '{salary.date_paid}', {salary.isPaid})";
                    DatabaseHelper.Instance.Update(query);
                }
                else
                {
                    string query = $@"UPDATE salary SET net_salary = {salary.net_salary}, deduction = {salary.deduction}
                                      where salary_id = {salary.salary_id}";
                    DatabaseHelper.Instance.Update(query);
                }
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting salary: " + ex.Message);
                return false;
            }
        }
        public static bool isExist(int id, string date)
        {
            int count = -1;
            try
            {
                string query = $"select count(*) from salary where teacher_id = {id} and strftime('%m', '{date}') = strftime('%m', date_paid)";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex);
            }
            return count > 0;
        }
        public static List<SalaryB> GetAllSalary(string month)
        {
            List<SalaryB> list = new List<SalaryB>();

                string query = $"SELECT salary_id, t.teacher_id, net_salary, bonus, deduction, date_paid, paid, t.name FROM salary Natural JOIN teachers t where strftime('%m', date_paid) = '{month}'";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query) ;
                    while (reader.Read())
                    {
                        list.Add(new SalaryB
                        {
                            salary_id = reader.GetInt32(0),
                            teacher = new TeacherB
                            {
                                id = reader.GetInt32(1),
                                name = reader.IsDBNull(7)? "" : reader.GetString(7),
                            },
                            net_salary = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                            bonus = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                            deduction = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                            date_paid = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            isPaid = !reader.IsDBNull(6) && reader.GetInt32(6) == 1
                        });
                    }
            return list;
        }

        public static bool Update(int id, bool paid, string date)
        {
            try
            {
                string name = string.Empty;
                string salary = string.Empty;

                string query = $"Update salary set paid = {paid}, date_paid = '{date}' where salary_id = {id}";
                DatabaseHelper.Instance.Update(query);
                if (paid == true)
                {
                    query = $"select name, salary from teachers t join salary s on t.teacher_id = s.teacher_id where salary_id = {id}";
                    SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                    if (reader.Read())
                    {
                        name = reader.GetString(0);
                        salary = reader.GetString(1);
                    }
                    reader.Close();

                    query = $"Insert into payments(PaymentType, Name, ToEntity, Contact, PaymentDate, Amount_paid) values('Salary', '{name}', 'Teacher', '{id}', '{date}', {salary})";
                    DatabaseHelper.Instance.Update(query);
                }
                else
                {
                    query = $"Delete from payments where contact = '{id}'";
                    DatabaseHelper.Instance.Update(query);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex);
                return false;
            }
        }
        public static DataTable GetSalaryReport(int month, int branch)
        {
            string query = $"Select * from salary_report where cast(strftime('%m', `Payment Date`) as INT) = {month}";
            DataTable dataTable = new DataTable();
            dataTable = DatabaseHelper.Instance.GetTable(query);
            return dataTable;
        }
    }
}
