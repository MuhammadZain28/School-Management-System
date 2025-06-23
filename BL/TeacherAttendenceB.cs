using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;

namespace LMS.BL
{
    internal class TeacherAttendenceB
    {
        public int attendence_id {  get; set; }
        public TeacherB teacher = new TeacherB();
        public string checkIn { get; set; }
        public string checkOut { get; set; }
        public double deduction {  get; set; }
        public string date {  get; set; }
        public string status { get; set; } 
        public int teacher_id => teacher.id;
        public string name => teacher.name;

        public List<TeacherAttendenceB> getData(string date)
        {
            return AttendenceD.getTeacherData(date);
        }
        public bool addData(List<TeacherAttendenceB> data)
        {
            return AttendenceD.addTeacherAttendence(data);
        }
        public List<TeacherAttendenceB> UpdateAttendanceIfExists(List<TeacherAttendenceB> list, int index, string newStatus, string newTime = "--:--:--")
        {
            if (index != -1)
            {
                list[index].status = newStatus;
                list[index].checkIn = newTime;
            }

            return list;
        }
        public List<TeacherAttendenceB> Out(List<TeacherAttendenceB> list, int index, string newTime)
        {
            if (index != -1)
            {
                list[index].checkOut = newTime;
            }
            return list;
        }
        public void Log()
        {
            if (AttendenceD.log())
            {
                MessageBox.Show("New Month Started!");
            }
        }
        public void ExportAttendanceToCSV(string filePath, int month)
        {
            List<TeacherAttendenceB> entries = AttendenceD.TeacherattendenceReport(month);
            var names = entries.Select(e => e.name).Distinct().OrderBy(n => n).ToList();
            var dates = entries.Select(e => e.date).Distinct().OrderBy(d => d).ToList();

            StringBuilder csv = new StringBuilder();

            csv.Append("Name");
            foreach (var date in dates)
                csv.Append($",{date:yyyy-MM-dd}");
            csv.AppendLine();

            foreach (var name in names)
            {
                csv.Append(name);
                foreach (var date in dates)
                {
                    var status = entries.FirstOrDefault(e => e.name == name && e.date == date)?.status ?? "";
                    csv.Append($",{status}");
                }
                csv.AppendLine();
            }

            File.WriteAllText(filePath, csv.ToString());
            MessageBox.Show("CSV file exported!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
