using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;
using Mysqlx.Datatypes;
using MessageBox = System.Windows.MessageBox;

namespace LMS.BL
{
    class TeacherB : Person
    {
        public string Joining { get; set; }
        public string Qualifications { get; set; }
        public string Institutes { get; set; }
        public string year { get; set; }
        public decimal salary { get; set; }
        public string designation { get; set; }
        public BatchesB branch  { get; set; } = new BatchesB();
        public string branch_name => branch.BranchName;

        
        public TeacherB() { }
        public TeacherB(int id, string name, string contact,string joining, string qualifications,string institutes, string year, decimal salary)
                        : base(id, name, contact)
        {
            Joining = joining;
            Qualifications = qualifications;
            Institutes = institutes;
            this.year = year;
            this.salary = salary;
        }
        public int Total()
        {
            return TeacherD.TotalTeacher();
        }
        public List<TeacherB> getData()
        {
            return TeacherD.TeacherData();
        }
        public bool deleteData(int id)
        {
            return TeacherD.delete_teacher(id);
        }
        public bool addData(TeacherB teacherbs, List<QualificationB> qualificationBs)
        {
            if ((teacherbs.name.Any(char.IsDigit)) && (teacherbs.name.Any(char.IsLetter)))
            {
                MessageBox.Show("Names Doesn't Contain Numbers and Contact Doesn't Contain Letters");
                return false;
            }
            else
            {
                if (teacherbs.salary <= 0)
                {
                    MessageBox.Show("Salary greater than '0'");
                    return false;
                }
                else
                {
                    if (TeacherD.add_teacher(teacherbs))
                    {
                        QualificationB qualificationB = new QualificationB();
                        if (qualificationBs == null || qualificationBs.Count == 0)
                            return true;
                        int id = TeacherD.get_id();
                        if (id > 0)
                        {
                            if(qualificationB.AddData(qualificationBs, id))
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            MessageBox.Show("Id Error" + id);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Teacher Not added");
                        return false;
                    }
                }
            }
        }
        public bool UpdateData(TeacherB teacherB, int Tid)
        {
            if (TeacherD.update_teacher(teacherB, Tid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public TeacherB SearchData(int id)
        {
            return TeacherD.TeacherSearch(id);
        }
        public Dictionary<int, string> GetTeachersbyCourse(int id)
        {
            List <TeacherB> teacherBs = TeacherD.GetTeachersByCourse(id);
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            foreach (var item in teacherBs)
            {
                keyValuePairs[item.id] = item.name;
            }
            return keyValuePairs;
        }
        public Dictionary<int, string> GetTeacherCombo()
        {
            List<TeacherB> teacherBs = TeacherD.GetTeacherCombo();
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            foreach (var item in teacherBs)
            {
                keyValuePairs[item.id] = item.name;
            }
            return keyValuePairs;
        }
        public DataTable GetDataTable()
        {
            return TeacherD.GetDataTable();
        }
    }
}
