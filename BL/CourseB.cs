using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;

namespace LMS.BL
{
    public class CourseB
    {
        public string courseName { get; set; }
        public int courseID { get; set; }
        public string Duration { get; set; } = "";
        public int TeacherId { get; set; }
        public string type { get; set; }
        public string teacherName { get; set; } = "";
        public void setCourseID(int id) { this.courseID = id;  }
        public int getCourseID() {  return courseID; }

        public Dictionary<int, string> loadBatch()
        {
            List<CourseB> list = CourseD.getData();
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            foreach (var item in list)
            {
                keyValuePairs[item.courseID] = item.courseName;
            }
            return keyValuePairs;
        }
        public bool Insert(CourseB courseB)
        {
            return CourseD.InsertCourse(courseB);    
        }
        public bool Delete(int id)
        {
            return CourseD.DeleteCourse(id);
        }
        public List<CourseB> Get()
        {
            return CourseD.getData();
        }
    }
}
