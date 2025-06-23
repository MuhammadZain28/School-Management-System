using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;

namespace LMS.BL
{
    internal class SalaryB
    {
        public TeacherB teacher = new TeacherB();
        public int salary_id { get; set; }
        public decimal salary => teacher.salary;
        public int teacher_id => teacher.id;
        public string name => teacher.name;
        public decimal net_salary { get; set; }
        public decimal deduction {  get; set; }
        public decimal bonus { get; set; }
        public bool isPaid { get; set; }
        public int total_present {  get; set; }
        public int total_absent {  get; set; }
        public int total_Leave { get; set; }
        public string date_paid { get; set; }

        public List<SalaryB> calculateSalary()
        {
            List<SalaryB> salaries = SalaryD.getData();
            int days = 0;

            if (DateTime.Now.Month == 1 || DateTime.Now.Month == 3 || DateTime.Now.Month == 5 || DateTime.Now.Month == 7 || DateTime.Now.Month == 8 || DateTime.Now.Month == 10 || DateTime.Now.Month == 12)
            {
                days = 31;
            }
            else if (DateTime.Now.Month == 2)
            {
                days = 28;
                if (DateTime.Now.Year % 4 == 0)
                {
                    days = 29;
                }
            }
            else
            {
                days = 30;
            }

            decimal salary_Per_Day = 0;
            foreach (var item in salaries)
            {
                item.date_paid = DateTime.Now.ToString("yyyy-MM-dd");
                salary_Per_Day = item.salary / days;
                item.deduction = salary_Per_Day * item.total_absent;
                item.net_salary = item.salary - item.deduction;
                SalaryD.Insert(item);
            }
            return salaries;
        }
        public List<SalaryB> GetSalaries(string month)
        {
            return SalaryD.GetAllSalary(month);
        }
    }
}
