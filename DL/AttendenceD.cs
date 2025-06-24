using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ControlzEx.Standard;
using LMS.BL;
using Microsoft.Data.Sqlite;
using WinFormsApp1;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    internal class AttendenceD
    {

        private static AttendenceD? instance = null;


        public static AttendenceD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AttendenceD();
                }
                return instance;
            }
        }
        public static bool addData(List<AttendenceB> a)
        {
            string formattedDate;
            if (!ifExist(a[0].Adate, a[0].classId, a[0].batchId))
            {
                foreach (var item in a)
                {
                    try
                    {
                        formattedDate = item.Adate;
                        string query = $@" INSERT INTO attendance (student_id, class_id, batch_id, status, date) VALUES ({item.studentId}, {item.classId}, {item.batchId}, '{item.status}', '{formattedDate}')";
                        DatabaseHelper.Instance.Update(query);

                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Error :" + e);
                        return false;
                    }
                }
            }
            else
            {
                foreach (var item in a)
                {
                    try
                    {
                        formattedDate = item.Adate;
                        string query = $@"UPDATE Attendance SET status = '{item.status}' WHERE class_id  = {item.classId} and student_id = {item.studentId} and date = '{item.Adate}' and batch_id = {item.batchId}";
                        DatabaseHelper.Instance.Update(query);

                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("" + e);
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool addTeacherAttendence(List<TeacherAttendenceB> a)
        {
            if (!ifTeacherExist(a[0].date))
            {
                foreach (var item in a)
                {
                    try
                    {
                        string query = $@" INSERT INTO teacher_attendence (teacher_id, status, date, check_In, check_Out) VALUES ({item.teacher.id}, '{item.status}', '{item.date}', '{item.checkIn}', '{item.checkOut}')";
                        DatabaseHelper.Instance.Update(query);

                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("" + e);
                        return false;
                    }
                }
            }
            else
            {
                foreach (var item in a)
                {
                    try
                    {
                        string query = $@"UPDATE teacher_attendence SET status = '{item.status}', check_In = '{item.checkIn}', check_out ='{item.checkOut}' WHERE date = '{item.date}' and teacher_id = {item.teacher.id}";
                        DatabaseHelper.Instance.Update(query);
                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Error: " + e);
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool Out(TeacherAttendenceB checkOut)
        {
            try
            {
                string query = $@"UPDATE teacher_attendence SET check_Out = '{checkOut.checkOut}' WHERE date = '{checkOut.date}' and teacher_id = {checkOut.teacher.id}";
                DatabaseHelper.Instance.Update(query);
            }

            catch (Exception e)
            {
                MessageBox.Show("Error: " + e);
                return false;
            }
            return true;
        }
        public static List<AttendenceB> getData(string date, int c_id, int b_id)
        {
            List<AttendenceB> attendenceBs = new List<AttendenceB>();
            try
            {
                string query = $"Select student_id, status from Attendance where date = '{date}' and class_id = {c_id} and batch_id = {b_id};";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                while (reader.Read())
                {
                    attendenceBs.Add(new AttendenceB
                    {
                        studentId = reader.GetInt32(0),
                        status = reader.GetChar(1),
                        classId = c_id,
                        batchId = b_id,
                        Adate = date,
                    });
                }
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error : " + e);
            }
            return attendenceBs;
        }
        public static List<TeacherAttendenceB> getTeacherData(string Date)
        {
            List<TeacherAttendenceB> attendenceBs = new List<TeacherAttendenceB>();
            try
            {
                string query = $"Select teacher_id, status, check_In from teacher_Attendence where date = '{Date}';";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                while (reader.Read())
                {
                    attendenceBs.Add(new TeacherAttendenceB
                    {
                        teacher = new TeacherB
                        {
                            id = reader.GetInt32(0),
                        },
                        status = reader.GetString(1),
                        date = Date,
                        checkIn = reader.GetString(2),
                    });
                }
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error : " + e);
            }
            return attendenceBs;
        }
        public static bool ifExist(string date, int c_id, int b_id)
        {
            int count = -1;
            try
            {
                string query = $"Select count(student_id) from Attendance where date = '{date}' and class_id = {c_id} and batch_id = {b_id};";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error : " + e);
            }
            return count > 0;
        }
        public static bool ifTeacherExist(string date)
        {
            int count = -1;
            try
            {
                string query = $"Select count(teacher_id) from Teacher_Attendence where date = '{date}';";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error : " + e);
            }
            return count > 0;
        }
        public static bool log()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            if (!checkLog(date))
            {
                try
                {
                    string query = $"Insert into monthly_trigger_control(trigger_date) values ('{date}')";
                    DatabaseHelper.Instance.Update(query);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error :" + e);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool checkLog(string date)
        {
            int count = -1;

            try
            {
                string query = $"SELECT COUNT(control_id) FROM monthly_trigger_control WHERE strftime('%m', trigger_date) = strftime('%m', '{date:MM}');";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error :" + e);
            }

            return count > 0;
        }
        public static DataTable attendenceReport(int month)
        {
            DataTable dt = new DataTable();
            string query = $"select * from monthly_attendance_report WHERE month = {month}";
            dt = DatabaseHelper.Instance.GetTable(query);
            return dt;
        }

        public static List<TeacherAttendenceB> TeacherattendenceReport(int month, int branch)
        {
            List<TeacherAttendenceB> dt = new List<TeacherAttendenceB>();
            MessageBox.Show("" + month);
            string query = $"select * from Teacher_attendence_report WHERE CAST(strftime('%m', date) as INT) = {month}";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                dt.Add(new TeacherAttendenceB()
                {
                    teacher = new TeacherB
                    {
                        name = reader.GetString(0),
                    },
                    status = reader.GetString(1),
                    date = reader.GetString(2),
                });
            }
            return dt;
        }
    }
}
