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
    class TeacherD
    {
        private static TeacherD? instance = null;


        public static TeacherD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TeacherD();
                }
                return instance;
            }
        }
        public static int TotalTeacher()
        {
            string query = $"Select count(teacher_id) from teachers;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            reader.Close();
            return count;
        }
        public static List<TeacherB> TeacherData() 
        {
            List<TeacherB> teacherBs = new List<TeacherB>();
            try
            {
                string query = @$"select t.teacher_id, t.name, t.contact, t.joining_date, GROUP_CONCAT(tq.degree_name, ', ') AS degrees, t.salary, t.designation, t.branch_id, b.branch_name
                            from teachers t
                            left join  teacher_qualifications tq ON t.teacher_id = tq.teacher_id
                            JOIN branch b on t.branch_id = b.branch_id
                            group by t.teacher_id, t.name, t.contact, t.joining_date
                            order by  t.teacher_id;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                while (reader.Read())
                {
                    teacherBs.Add(new TeacherB()
                    {
                        id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        contact = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Joining = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("dd-MM-yyyy"),
                        Qualifications = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        salary = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                        designation = reader.IsDBNull(6) ? "" as string : reader.GetString(6),
                        branch = new BatchesB
                        {
                            branchId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                            BranchName = reader.IsDBNull(8) ? "" : reader.GetString(8),
                        }
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
            return teacherBs ;
        }

        public static TeacherB TeacherSearch(int id)
        {
            TeacherB teacherBs = new TeacherB();
            string query = @$"select t.teacher_id, t.name, t.contact, t.joining_date, GROUP_CONCAT(tq.degree_name, ', '), 
                            GROUP_CONCAT(tq.Institute, ', '), GROUP_CONCAT(tq.year, ', '), t.salary, t.designation, t.branch_id, b.branch_name
                            from teachers t
                            left join  teacher_qualifications tq ON t.teacher_id = tq.teacher_id
                            JOIN branch b on t.branch_id = b.branch_id
                            where t.teacher_id = {id}
                            group by t.teacher_id, t.name, t.contact, t.joining_date
                            order by  t.teacher_id;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                teacherBs.id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                teacherBs.name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                teacherBs.contact = reader.IsDBNull(2) ? "" : reader.GetString(2);
                teacherBs.Joining = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("dd-MM-yyyy");
                teacherBs.Qualifications = reader.IsDBNull(4) ? "" : reader.GetString(4);
                teacherBs.Institutes = reader.IsDBNull(5) ? "" : reader.GetString(5);
                teacherBs.year = reader.IsDBNull(6) ? "" : reader.GetString(6);
                teacherBs.salary = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7);
                teacherBs.designation = reader.IsDBNull(8) ? "" as string : reader.GetString(8);
                teacherBs.branch = new BatchesB
                {
                    branchId = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                    BranchName = reader.IsDBNull(10) ? "" : reader.GetString(10),
                };
            }
            reader.Close();
            return teacherBs;
        }
        public static bool update_teacher(TeacherB teacher, int id)
        {
            try
            {
                string query = $@"update teachers set salary = {teacher.salary}, contact = '{teacher.contact}' where teacher_id = {id};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return false;
            }
        }
        public static bool delete_teacher(int id)
        {
            try
            {
                string query = $"delete from teachers where teacher_id = {id};";
                DatabaseHelper.Instance.getData(query);
                return true;
            }
            catch 
            { 
                return false; 
            }
        }
        public static bool add_teacher(TeacherB teacher)
        {
            DateTime parsedDate = DateTime.Parse(teacher.Joining);
            string formattedDate = parsedDate.ToString("yyyy-MM-dd");

            try
            {
                string query = $@"INSERT INTO teachers (name, contact, joining_date, salary, designation, branch_id) VALUES
                                  ('{teacher.name}', '{teacher.contact}', '{formattedDate}', {teacher.salary}, '{teacher.designation}', {teacher.branch.branchId});";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("" + ex);
                return false;
            }
        }
        public static int get_id()
        {
            int id = -1;
            string query = "select teacher_id from teachers order by teacher_id desc Limit 1;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            reader.Close();
            return id;
        }
        public static List<TeacherB> GetTeachersByCourse(int courseId)
        {
            List<TeacherB> teacherList = new List<TeacherB>();
            string query = $@"SELECT t.teacher_id, t.name 
                      FROM teachers t 
                      JOIN courses c ON c.teacher_id = t.teacher_id 
                      WHERE c.course_id = {courseId};";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                teacherList.Add(new TeacherB()
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1)
                });
            }
            reader.Close();
            return teacherList;
        }
        public static List<TeacherB> GetTeacherCombo()
        {
            List<TeacherB> teacherList = new List<TeacherB>();
            string query = $@"SELECT t.teacher_id, t.name 
                      FROM teachers t ;";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                teacherList.Add(new TeacherB()
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1)
                });
            }
            reader.Close();
            return teacherList;
        }

        public static DataTable GetDataTable()
        {
            DataTable dt = new DataTable();
            string query = $"select teacher_performance_report;";
            dt = DatabaseHelper.Instance.GetTable(query);
            return dt;
        }
    }
}
