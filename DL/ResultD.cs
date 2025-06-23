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
    internal class ResultD
    {
        private static ResultD? instance = null;


        public static ResultD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResultD();
                }
                return instance;
            }
        }
        public static List<ResultB> GetResults()
        {
            List<ResultB> results = new List<ResultB>();
            string query = @"SELECT r.result_id, s.student_id, s.name, r.obtained_marks, r.grade, r.remarks, r.exam_id, c.course_id, c.course_name, cl.class_id, cl.class_name, s.roll_no, e.total_marks FROM results r join exam_schedule e on e.exam_id = r.exam_id join students s on s.student_id = r.student_id join courses c on c.course_id = e.course_id join classes cl on cl.class_id = e.class_id;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                results.Add(new ResultB()
                {
                    ResultId = reader.GetInt32(0),
                    Students = new StudentB()
                    {
                        id = reader.GetInt32(1),
                        name = reader.GetString(2),
                        roll = reader.GetString(11)
                    },
                    ObtainedMarks = reader.GetInt32(3),
                    Grade = reader.GetString(4),
                    Remarks = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Exams = new ExamB()
                    {
                        ExamId = reader.GetInt32(6),
                        Classes = new ClassB()
                        {
                            classId = reader.GetInt32(7),
                            name = reader.GetString(8)
                        },
                        Courses = new CourseB()
                        {
                            courseID = reader.GetInt32(9),
                            courseName = reader.GetString(10)
                        },
                        TotalMarks = reader.GetInt32(12),
                    },
                    
                });
            }
            reader.Close();
            return results;
        }
        public static bool InsertResult(ResultB result)
        {
            try
            {
                string query = $@"INSERT INTO results (student_id, obtained_marks, grade, remarks, exam_id) VALUES ({result.Students.id}, {result.ObtainedMarks}, '{result.Grade}', '{result.Remarks}', {result.Exams.ExamId});";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        public static bool UpdateResult(ResultB result, int id)
        {
            try
            {
                string query = $@"UPDATE results SET obtained_marks = {result.ObtainedMarks},  grade = '{result.Grade}', remarks = '{result.Remarks}', exam_id = {result.Exams.ExamId} WHERE result_id = {id};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        public static ResultB SearchResult(int resultId)
        {
            ResultB result = null;
            string query = $@"SELECT r.result_id, s.student_id, s.name, r.obtained_marks, r.grade, r.remarks, r.exam_id, c.course_id, c.course_name, cl.class_id, cl.class_name, s.roll_no, e.total_marks FROM results r
                            JOIN exam_schedule e ON e.exam_id = r.exam_id
                            JOIN students s ON s.student_id = r.student_id
                            JOIN courses c ON c.course_id = e.course_id
                            JOIN classes cl ON cl.class_id = e.class_id
                            WHERE r.result_id = {resultId};";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                result = new ResultB()
                {
                    ResultId = reader.GetInt32(0),
                    Students = new StudentB()
                    {
                        id = reader.GetInt32(1),
                        name = reader.GetString(2),
                        roll = reader.GetString(11),
                    },
                    ObtainedMarks = reader.GetInt32(3),
                    Grade = reader.GetString(4),
                    Remarks = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Exams = new ExamB()
                    {
                        ExamId = reader.GetInt32(6),
                        Courses = new CourseB()
                        {
                            courseID = reader.GetInt32(7),
                            courseName = reader.GetString(8)
                        },
                        Classes = new ClassB()
                        {
                            classId = reader.GetInt32(9),
                            name = reader.GetString(10)
                        },
                        TotalMarks = reader.GetInt32(12),
                    }
                };
            }
            reader.Close();
            return result;
        }
        public static List<ResultB> FilterResultsByClass(int classId)
        {
            List<ResultB> results = new List<ResultB>();
            string query = $@"SELECT r.result_id, s.student_id, s.name, r.obtained_marks, r.grade, r.remarks, r.exam_id, c.course_id, c.course_name, cl.class_id, cl.class_name,s.roll_no, e.total_marks
        FROM results r
        JOIN exam_schedule e ON e.exam_id = r.exam_id
        JOIN students s ON s.student_id = r.student_id
        JOIN courses c ON c.course_id = e.course_id
        JOIN classes cl ON cl.class_id = e.class_id
        WHERE cl.class_id = {classId};";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                results.Add(new ResultB()
                {
                    ResultId = reader.GetInt32(0),
                    Students = new StudentB()
                    {
                        id = reader.GetInt32(1),
                        name = reader.GetString(2),
                        roll = reader.GetString(11),
                    },
                    ObtainedMarks = reader.GetInt32(3),
                    Grade = reader.GetString(4),
                    Remarks = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Exams = new ExamB()
                    {
                        ExamId = reader.GetInt32(6),
                        Courses = new CourseB()
                        {
                            courseID = reader.GetInt32(7),
                            courseName = reader.GetString(8)
                        },
                        Classes = new ClassB()
                        {
                            classId = reader.GetInt32(9),
                            name = reader.GetString(10)
                        },
                        TotalMarks = reader.GetInt32(12),
                    }
                });
            }
            reader.Close();
            return results;
        }
        public static bool DeleteResult(int resultId)
        {
            try
            {
                string query = $@"DELETE FROM results WHERE result_id = {resultId}";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        public static DataTable GetDataTable(int year)
        {
            DataTable dt = new DataTable();
            string query = $"select * from academic_year_student_performance WHERE \"Academic year\" = 2025";
            dt = DatabaseHelper.Instance.GetTable(query);
            return dt;
        }
    }
}
