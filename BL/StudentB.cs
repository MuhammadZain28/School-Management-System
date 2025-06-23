using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using LMS.DL;
using MessageBox = System.Windows.MessageBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LMS.BL
{
    class StudentB : Person
    {
        public string roll { get; set; }
        public decimal fees {  get; set; }
        public string address { get; set; }
        public string admission_date { get; set; }
        public string batch_name { get; set; }
        public string class_name { get; set; }

        public StudentB() { }

        public StudentB(int id, string name, string contact,string roll, decimal fee, string address,string admissionDate, string batchName,string className)
                        : base(id, name, contact)
        {
            this.roll = roll;
            this.fees = fee;
            this.address = address;
            this.admission_date = admissionDate;
            this.batch_name = batchName;
            this.class_name = className;
        }


        StudentD student = new StudentD();

        public bool addStudent(int Cid, int Bid)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contact) ||
                string.IsNullOrEmpty(address) || string.IsNullOrEmpty(admission_date) || string.IsNullOrEmpty(batch_name))
            {
                MessageBox.Show("Please fill in all fields with valid information.");
                return false;
            }

            bool success = StudentD.addStudent(name, roll, fees, contact, address, admission_date, Bid, Cid);
            if (success) 
            {
                MessageBox.Show("Student added successfully.");
                return true;
            }
            return success;
        }
        public bool Updatedata(StudentB s, int Cid, int id)
        {
            return StudentD.UpdateStudent(s, Cid, id);
        }
        public int totalStudents()
        {
            return StudentD.totalStudent();
        }

        public List<int> fee()
        {
            List<int> total = new List<int>();
            total.Add(StudentD.totalMaleStudent());
            total.Add(StudentD.totalFemaleStudent());
            return total;
        }
        public List<StudentB> getData()
        {
            return StudentD.StudentData(); 
        }
        public bool deleteData(int id)
        {
            return StudentD.delete(id);
        }
        public StudentB searchData(int id)
        {
            return StudentD.searchStudent(id);
        }
        public List<StudentB> filterData(int id)
        {
            return StudentD.filterStudent(id);
        }
        public List<StudentB> attend(int bId, int cId)
        {
            return StudentD.attendence(bId, cId);
        }
        public Dictionary<int, string> IdRollPairs(int id)
        {
            List<StudentB> studentBs = StudentD.filterClassStudent(id);
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            foreach (var item in studentBs)
            {
                keyValuePairs[item.id] = item.roll;
            }
            return keyValuePairs;
        }
    }
}
