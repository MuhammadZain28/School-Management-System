using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;
using MessageBox = System.Windows.MessageBox;

namespace LMS.BL
{
    internal class ScheduleB
    {
        public int ScheduleId { get; set; }
        public string DayOfWeek { get; set; }
        public string Time { get; set; }
        public TimeSpan StartTime { get; set; } 
        public TimeSpan EndTime { get; set; }
        public TeacherB Teachers { get; set; }
        public ClassB Classes { get; set; }
        public CourseB Courses { get; set; }
        public string courseName => Courses.courseName; 
        public string className => Classes.name;
        public string teacherName => Teachers.name; 

        public List<ScheduleB> Schedules()
        {
            return ScheduleD.GetClassSchedules();
        }
        public List<ScheduleB> FilterSchedule(int id)
        {
            return ScheduleD.FilterClassSchedules(id);
        }
        public bool DeleteClass(int id)
        {
            return ScheduleD.DeleteSchedule(id);
        }
        public bool AddSchedule(HashSet<ScheduleB> schedules)
        {
            return ScheduleD.InsertSchedule(schedules);
        }
        public bool checkStartEnd(TimeSpan start, TimeSpan end, int id, string day)
        {
            if (start < end)
            {
                if (ScheduleD.checkClass(start, end, id, day))
                {
                    MessageBox.Show("Classes Conflicts");
                    return false;
                }
                else
                    return true;
            }
            return false;
        }
    }
}
