using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LMS.BL;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;

namespace LMS.DL
{
    internal class CourseD
    {

        private static CourseD? instance = null;


        public static CourseD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CourseD();
                }
                return instance;
            }
        }
        public static List<CourseB> getData()
        {
            List<CourseB> courseList = new List<CourseB>();
            try
            {
                string query = "SELECT course_id, course_name, type, c.teacher_id, t.name FROM courses c join teachers t on c.teacher_id = t.teacher_id";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    courseList.Add(new CourseB()
                    {
                        courseID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        courseName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        type = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        TeacherId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        teacherName = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
            return courseList;
        }
        public static bool InsertCourse(CourseB course)
        {
            try
            {
                string query = $@"INSERT INTO courses (course_name, teacher_id, type) 
                        VALUES ('{course.courseName}', {course.TeacherId}, '{course.type}');";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("InsertCourse Error: " + ex.Message);
                return false;
            }
        }
        public static bool DeleteCourse(int id)
        {
            try
            {
                string query = $@"Delete from courses where course_id = {id};";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("InsertCourse Error: " + ex.Message);
                return false;
            }
        }
    }
}
