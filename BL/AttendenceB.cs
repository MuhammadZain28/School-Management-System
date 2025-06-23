using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlzEx.Standard;
using System.Windows;
using LMS.DL;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Documents;
using System.Data;
using Microsoft.Data.Sqlite;

namespace LMS.BL
{
    internal class AttendenceB
    {
        public int studentId {  get; set; }
        public int batchId { get; set; }
        public int classId { get; set; }
        public char status { get; set; }
        public string Adate { get; set; }

        AttendenceD attendence = new AttendenceD();

        public bool addAttendence(List<AttendenceB> attences)
        {
            return AttendenceD.addData(attences);
        }
        public int count(char status, DateTime date)
        {

            int count = 0;
            try
            {
                string query = $"select count(attendance_id) from attendance where status = '{status}' and date = '{date.ToString("yyyy-MM-dd")}'";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
                if (reader.Read())
                    count = reader.GetInt32(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            return count;
        }
        public List<AttendenceB> UpdateAttendanceIfExists(List<AttendenceB> list, int index, char newStatus)
        {
            if (index != -1)
            {
                list[index].status = newStatus;
            }

            return list;
        }
        public DataTable GetDataTable(int month)
        {
            return AttendenceD.attendenceReport(month);
        }
    }
}
