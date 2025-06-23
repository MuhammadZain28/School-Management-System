using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using LMS.BL;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    class StudentD
    {
        private static StudentD? instance = null;


        public static StudentD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentD();
                }
                return instance;
            }
        }
        public static bool addStudent( string name, string roll, decimal fee, string contact, string address, string adminssion_date, int Bid, int id)
        {
            
            try
            {
                string query = $"INSERT INTO students (name, roll_no, fee, contact, address, admission_date, batch_id, class_id) " +
               $"VALUES ('{name}', '{roll}', '{fee}', '{contact}', '{address}', '{adminssion_date}', {Bid}, {id});";

                int result = DatabaseHelper.Instance.Update(query);

                return result > 0;  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        public static List<StudentB> StudentData()
        {
            List<StudentB> studentBs = new List<StudentB>();
            try
            {
                string query = @$"SELECT 
                            s.name, 
                            s.fee, 
                            s.contact, 
                            s.address, 
                            s.admission_date, 
                            b.Branch_name, 
                            s.age,
                            s.roll_no,
                            c.class_name,
                            s.student_id
                        FROM students s
                        JOIN Branch b ON s.batch_id = b.Branch_id
                        JOIN classes c ON c.class_id = s.class_id
                         order by s.student_id asc;
                        ";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    StudentB student = new StudentB
                    {
                        name = reader.GetString(0),
                        fees = reader.GetDecimal(1),
                        contact = reader.IsDBNull(2) ? "03XX-XXXXXXXXX" : reader.GetString(2),
                        address = reader.GetString(3),
                        admission_date = reader.GetDateTime(4).ToString("dd-MM-yyyy"),
                        batch_name = reader.GetString(5),
                        roll = reader.GetString(7),
                        class_name = reader.GetString(8),
                        id = reader.GetInt32(9),
                    };

                    studentBs.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex);
            }
            return studentBs;
        }
        public static StudentB searchStudent(int studentId)
        {
            StudentB student = null;  
            try
            {
                string query = @$"SELECT 
                                        s.name, 
                                        s.fee, 
                                        s.contact, 
                                        s.address, 
                                        s.admission_date, 
                                        b.Branch_name, 
                                        s.age,
                                        s.roll_no
                                    FROM students s
                                    JOIN Branch b ON s.batch_id = b.Branch_id
                                    WHERE s.student_id = {studentId}";  
            
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                if (reader.Read())
                {
                    student = new StudentB
                    {
                        name = reader.GetString(0),
                        fees = reader.GetDecimal(1),
                        contact = reader.IsDBNull(2) ? "03XX-XXXXXXXXX" : reader.GetString(2),
                        address = reader.GetString(3),
                        admission_date = reader.GetDateTime(4).ToString("dd-MM-yyyy"),
                        batch_name = reader.GetString(5),
                        roll = reader.GetString(7),
                    };
                }

                reader.Close();  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return student;
        }
        public static List<StudentB> filterStudent(int bId)
        {
            List<StudentB> student = new List<StudentB>();
            try
            {
                string query = @$"SELECT 
                                        s.name, 
                                        s.fee, 
                                        s.contact, 
                                        s.address, 
                                        s.admission_date, 
                                        b.Branch_name, 
                                        s.age,
                                        s.roll_no
                                    FROM students s
                                    JOIN Branch b ON s.batch_id = b.Branch_id
                                    WHERE s.class_id = {bId}";

                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    student.Add(new StudentB
                    {
                        name = reader.GetString(0),
                        fees = reader.GetDecimal(1),
                        contact = reader.IsDBNull(2) ? "03XX-XXXXXXXXX" : reader.GetString(2),
                        address = reader.GetString(3),
                        admission_date = reader.GetDateTime(4).ToString("dd-MM-yyyy"),
                        batch_name = reader.GetString(5),
                        roll = reader.GetString(7)
                    });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return student;
        }
        public static List<StudentB> filterClassStudent(int cId)
        {
            List<StudentB> student = new List<StudentB>();
            try
            {
                string query = @$"SELECT 
                                        s.roll_no, 
                                        s.student_id
                                    FROM students s
                                    JOIN Branch b ON s.batch_id = b.Branch_id
                                    WHERE s.class_id = {cId}";

                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    student.Add(new StudentB
                    {
                        roll = reader.GetString(0),
                        id = reader.GetInt32(1)
                    });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return student;
        }
        public static List<StudentB> attendence(int bId, int cId)
        {
            List<StudentB> student = new List<StudentB>();
            try
            {
                string query = @$"SELECT 
                                        s.name, 
                                        s.fee, 
                                        s.contact, 
                                        s.address, 
                                        s.admission_date, 
                                        b.Branch_name, 
                                        s.age,
                                        s.roll_no,
                                        s.student_id
                                    FROM students s
                                    JOIN Branch b ON s.batch_id = b.Branch_id
                                    WHERE s.batch_id = {bId} AND s.class_id = {cId}";

                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    student.Add(new StudentB
                    {
                        name = reader.GetString(0),
                        fees = reader.GetDecimal(1),
                        contact = reader.GetString(2),
                        address = reader.GetString(3),
                        admission_date = reader.GetDateTime(4).ToString("dd-MM-yyyy"),
                        batch_name = reader.GetString(5),
                        roll = reader.GetString(7),
                        id = reader.GetInt32(8)
                    });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return student;
        }
        public static bool UpdateStudent(StudentB s, int cid, int Id)
        {
            try
            {
                string query = @$"UPDATE students SET contact = '{s.contact}', address = '{s.address}', roll_no = '{s.roll}', class_id = {cid} WHERE student_id = {Id};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch
            { return false; }
        }
        public static int getBatchID(string batch_name)
        {
            try
            {
                string query = $"SELECT Branch_id FROM Branch WHERE Branch_name = '{batch_name}';";

                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                int batchId = -1;

                if (reader.Read())
                {
                    batchId = reader.GetInt32(0);
                }

                reader.Close();

                return batchId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return -1;
            }
        }

        public static int totalStudent()
        {
            string query = $"Select count(student_id) from students;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            reader.Close();
            return count;
        }

        public static int totalMaleStudent()
        {
            string query = $"Select count(student_id) from students Where fee = 'Male';";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            reader.Close();
            return count;
        }
        public static int totalFemaleStudent()
        {
            string query = $"Select count(student_id) from students Where fee = 'Female';";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            reader.Close();
            return count;
        }
        public static bool delete(int id)
        {
            try
            {
                string query = $"delete from students where student_id = {id}";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception e) 
            {
                MessageBox.Show("" + e);
                return false;
            }
        }

        public static int stdId(string roll)
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
        

    }
}
