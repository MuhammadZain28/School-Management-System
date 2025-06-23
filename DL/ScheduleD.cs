using System;
using System.Collections;
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
    internal class ScheduleD
    {
        public static List<ScheduleB> GetClassSchedules()
        {
            List<ScheduleB> schedules = new List<ScheduleB>();

            string query = @"SELECT  cs.schedule_id,  cs.day_of_week,  concat(cs.start_time, ' - ', cs.end_time),  t.teacher_id,  t.name, cl.class_id,  cl.class_name, c.course_id,  c.course_name FROM class_schedule cs JOIN classes cl ON cl.class_id = cs.class_id JOIN courses c ON c.course_id = cs.course_id JOIN teachers t ON t.teacher_id = c.teacher_id;";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

            while (reader.Read())
            {
                schedules.Add(new ScheduleB()
                {
                    ScheduleId = reader.GetInt32(0),
                    DayOfWeek = reader.GetString(1),
                    Time = reader.GetString(2),
                    

                    Teachers = new TeacherB()
                    {
                        id = reader.GetInt32(3),
                        name = reader.GetString(4)
                    },

                    Classes = new ClassB()
                    {
                        classId = reader.GetInt32(5),
                        name = reader.GetString(6)
                    },

                    Courses = new CourseB()
                    {
                        courseID = reader.GetInt32(7),
                        courseName = reader.GetString(8)
                    }
                });
            }

            reader.Close();
            return schedules;
        }
        public static List<ScheduleB> FilterClassSchedules(int id)
        {
            List<ScheduleB> schedules = new List<ScheduleB>();

            string query = @$"SELECT  cs.schedule_id,  cs.day_of_week,  concat(cs.start_time, ' - ', cs.end_time),  t.teacher_id,  t.name, cl.class_id,  cl.class_name, c.course_id,  c.course_name FROM class_schedule cs JOIN classes cl ON cl.class_id = cs.class_id JOIN courses c ON c.course_id = cs.course_id JOIN teachers t ON t.teacher_id = c.teacher_id WHERE cs.class_id = {id};";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

            while (reader.Read())
            {
                schedules.Add(new ScheduleB()
                {
                    ScheduleId = reader.GetInt32(0),
                    DayOfWeek = reader.GetString(1),
                    Time = reader.GetString(2),


                    Teachers = new TeacherB()
                    {
                        id = reader.GetInt32(3),
                        name = reader.GetString(4)
                    },

                    Classes = new ClassB()
                    {
                        classId = reader.GetInt32(5),
                        name = reader.GetString(6)
                    },

                    Courses = new CourseB()
                    {
                        courseID = reader.GetInt32(7),
                        courseName = reader.GetString(8)
                    }
                });
            }

            reader.Close();
            return schedules;
        }
        public static bool DeleteSchedule(int scheduleId)
        {
            try
            {
                string query = $@"DELETE FROM class_schedule WHERE schedule_id = {scheduleId};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch
            {  return false; }
        }
        public static bool InsertSchedule(HashSet<ScheduleB> scheduleBs)
        {
            string query = "";
            foreach (var item in scheduleBs)
            {
                try
                {
                    if (!checkClass(item.StartTime, item.EndTime, item.Classes.classId, item.DayOfWeek))
                    {
                        try
                        {
                            query = @$"INSERT INTO class_schedule (day_of_week, start_time, end_time, teacher_id, class_id, course_id)
                                VALUES ('{item.DayOfWeek}', '{item.StartTime.ToString(@"hh\:mm")}', '{item.EndTime.ToString(@"hh\:mm")}', {item.Teachers.id}, {item.Classes.classId}, {item.Courses.courseID});";
                            DatabaseHelper.Instance.Update(query);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Class is already Scheduled on this time");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                    return false;
                }
            }
            return true;
        }
        public static bool checkClass(TimeSpan start, TimeSpan end, int id, string day)
        {
            int count = 0;
            string query = @$"SELECT COUNT(schedule_id) FROM class_schedule WHERE class_id = {id} AND day_of_week = '{day}'
                              AND NOT (
                                  end_time <= '{start}' OR start_time >= '{end}'
                              );";

            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
                count = reader.GetInt32(0);
            reader.Close();
            return (count > 0);
        }
    }
}
