using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;

namespace LMS.BL
{
    internal class ResultB
    {
        public int ResultId { get; set; }
        public StudentB Students { get; set; } = new StudentB();
        public int ObtainedMarks { get; set; }
        public string Grade { get; set; }
        public string Remarks { get; set; }
        public ExamB Exams { get; set; } = new ExamB();

        public string StudentName => Students.name;
        public string RollNo => Students.roll;  
        public string CourseName => Exams.CourseName;
        public string ClassName => Exams.className;
        public int TotalMarks => Exams.TotalMarks;
        public List<ResultB> GetResults()
        {
            return ResultD.GetResults();
        }
        public bool AddResult(ResultB result)
        {
            FeeD fee = new FeeD();
            result.Students.id = fee.stdId(RollNo);
            return ResultD.InsertResult(result);
        }
        public bool DeleteResult(int id)
        {
            return ResultD.DeleteResult(id);
        }
        public bool UpdateResult(ResultB result, int id)
        {
            return ResultD.UpdateResult(result, id);
        }
        public ResultB SearchResult(int id)
        {
            return ResultD.SearchResult(id);
        }
        public List<ResultB> FilterResult(int id)
        {
            return ResultD.FilterResultsByClass(id);
        }
        public DataTable GetDataTable(int year)
        {
            return ResultD.GetDataTable(year);
        }
    }
}
