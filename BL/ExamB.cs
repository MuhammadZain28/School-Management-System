using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;

namespace LMS.BL
{
    internal class ExamB
    {
        public int ExamId { get; set; }           
        public CourseB Courses { get; set; } = new CourseB();   
        public ClassB Classes { get; set; } = new ClassB();
        public DateTime Date { get; set; }        
        public int TotalMarks { get; set; }       
        public string Duration { get; set; }
        public string CourseName => Courses.courseName;
        public string className => Classes.name;
        public List<ExamB> GetExams()
        {
            return ExamD.GetExam();
        }
        public bool AddExam(ExamB exam)
        {
            return ExamD.InsertExam(exam);
        }
        public ExamB SearchExam(int id)
        {
            return ExamD.SearchExam(id);
        }
        public bool DeleteExam(int id)
        {
            return ExamD.DeleteExam(id);
        }
        public bool UpdateExam(ExamB exam, int ID)
        {
            return ExamD.updateExam(exam, ID);
        }
        public List<ExamB> filter(int id)
        {
            return ExamD.FilterExam(id);
        }
    }
}
