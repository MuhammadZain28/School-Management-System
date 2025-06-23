using System;
using System.Collections.Generic;
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
    internal class ExamD
    {
        private static ExamD? instance = null;


        public static ExamD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExamD();
                }
                return instance;
            }
        }
        public static List<ExamB> GetExam()
        {
            List<ExamB> examBs = new List<ExamB>();
            string query = $@"select e.exam_id, c.course_id, c.course_name, e.date, e.total_marks, e.duration, cl.class_id, cl.class_name 
                            from exam_schedule e 
                            join courses c on c.course_id = e.course_id 
                            join classes cl on cl.class_id = e.class_id;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                examBs.Add(new ExamB()
                {
                    ExamId = reader.GetInt32(0),
                    Courses = new CourseB()
                    {
                        courseID = reader.GetInt32(1),
                        courseName = reader.GetString(2),
                    },
                    Date = reader.GetDateTime(3),
                    TotalMarks = reader.GetInt32(4),
                    Duration = reader.GetString(5),
                    Classes = new ClassB()
                    {
                        classId = reader.GetInt32(6),
                        name = reader.GetString(7),
                    },
                });
            }
            reader.Close();
            return examBs;
        }
        public static ExamB SearchExam(int id)
        {
            ExamB examBs = new ExamB();
            string query = $@"select e.exam_id, c.course_id, c.course_name, e.date, e.total_marks, e.duration, cl.class_id, cl.class_name 
                            from exam_schedule e 
                            join courses c on c.course_id = e.course_id 
                            join classes cl on cl.class_id = e.class_id where exam_id = {id};";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                examBs = new ExamB()
                {
                    ExamId = reader.GetInt32(0),
                    Courses = new CourseB()
                    {
                        courseID = reader.GetInt32(1),
                        courseName = reader.GetString(2),
                    },
                    Date = reader.GetDateTime(3),
                    TotalMarks = reader.GetInt32(4),
                    Duration = reader.GetString(5),
                    Classes = new ClassB()
                    {
                        classId = reader.GetInt32(6),
                        name = reader.GetString(7),
                    },
                };
            }
            reader.Close();
            return examBs;
        }
        public static List<ExamB> FilterExam(int id)
        {
            List<ExamB> examBs = new List<ExamB>();
            string query = $@"select e.exam_id, c.course_id, c.course_name, e.date, e.total_marks, e.duration, cl.class_id, cl.class_name 
                            from exam_schedule e 
                            join courses c on c.course_id = e.course_id 
                            join classes cl on cl.class_id = e.class_id where e.class_id = {id};";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                examBs.Add( new ExamB()
                {
                    ExamId = reader.GetInt32(0),
                    Courses = new CourseB()
                    {
                        courseID = reader.GetInt32(1),
                        courseName = reader.GetString(2),
                    },
                    Date = reader.GetDateTime(3),
                    TotalMarks = reader.GetInt32(4),
                    Duration = reader.GetString(5),
                    Classes = new ClassB()
                    {
                        classId = reader.GetInt32(6),
                        name = reader.GetString(7),
                    },
                });
            }
            reader.Close();
            return examBs;
        }
        public static bool InsertExam(ExamB exam)
        {
            try
            {
                string query = $@"INSERT INTO exam_schedule (course_id, class_id, date, total_marks, duration) VALUES ({exam.Courses.courseID}, {exam.Classes.classId}, '{exam.Date:yyyy-MM-dd}', {exam.TotalMarks}, '{exam.Duration}')";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
                return false;
            }
        }
        public static bool updateExam(ExamB exam, int id)
        {
            try
            {
                string query = $@"UPDATE exam_schedule SET date = '{exam.Date:yyyy-MM-dd}', total_marks = {exam.TotalMarks}, duration = '{exam.Duration}' WHERE exam_id = {id};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
                return false;
            }
        }
        public static bool DeleteExam(int examId)
        {
            try
            {
                string query = $@"DELETE FROM exam_schedule WHERE exam_id = {examId}";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch
            { return false; }
        }
    }
}
