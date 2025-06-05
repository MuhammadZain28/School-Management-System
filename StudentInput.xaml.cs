using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Protobuf;
using LMS.BL;
using LMS.DL;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    public partial class StudentInput : Page
    {
        public int ID;
        public StudentInput(int id =-1, bool edit = false)
        {
            InitializeComponent();
            loadComboBoxClass();
            loadComboBoxBatch();
            ID = id;
            if (ID > 0)
            {
                view();
                if(edit)
                    Update();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (ID < 0)
            {
                string gender = "";
                if (Male.IsChecked == true)
                    gender = Male.Content.ToString();
                else if (Female.IsChecked == true)
                    gender = Female.Content.ToString();

                var selectedBatch = batch.SelectedItem as KeyValuePair<int, string>?;
                var selectedClass = @class.SelectedItem as KeyValuePair<int, string>?;

                if (selectedBatch == null && selectedClass == null)
                {
                    MessageBox.Show("Please select a class.");
                    return;
                }

                string batchName = selectedBatch.Value.Value;
                string className = selectedClass.Value.Value;
                DateTime selectedDate = Admission.SelectedDate ?? DateTime.Now;
                StudentB studentB = new StudentB(0,name.Text, contact.Text, Roll.Text, gender, address.Text, selectedDate.ToString("yyyy-MM-dd"), batchName, className);

                if (studentB.addStudent(selectedClass.Value.Key, selectedBatch.Value.Key))
                {
                    MainFrame.Navigate(new Student());
                }

            }
            else
            {
                int class_id = 0;
                if (@class.SelectedItem != null)
                {
                    var selectedClass = @class.SelectedItem as KeyValuePair<int, string>?;
                    class_id = selectedClass.Value.Key;
                }

                StudentB studentB = new StudentB();
                studentB.roll = Roll.Text;
                studentB.contact = contact.Text;
                studentB.address = address.Text;

                if (studentB.Updatedata(studentB, class_id, ID))
                {
                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    MessageBox.Show("Unsuccessfull");
                }
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private Dictionary<int, string> loadComboBoxBatch()
        {
            Dictionary<int, string> batches = new Dictionary<int, string>();
            try
            {
                string query = "SELECT batch_id, batch_name FROM batches;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    batches.Add(reader.GetInt32(0), reader.GetString(1));
                }
                reader.Close();

                batch.ItemsSource = batches;
                batch.DisplayMemberPath = "Value";
                batch.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return batches;
        }
        private Dictionary<int, string> loadComboBoxClass()
        {
            Dictionary<int, string> batches = new Dictionary<int, string>();
            try
            {
                string query = "SELECT class_id, class_name FROM classes;";
                SqliteDataReader reader = DatabaseHelper.Instance.getData(query);

                while (reader.Read())
                {
                    batches.Add(reader.GetInt32(0), reader.GetString(1));
                }
                reader.Close();

                @class.ItemsSource = batches;
                @class.DisplayMemberPath = "Value";
                @class.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return batches;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Student());
        }
        public void view()
        {
            StudentB student = new StudentB();
            student = student.searchData(ID);

            name.Text = student.name;
            contact.Text = student.contact;
            Roll.Text = student.roll;
            Dictionary<int, string> batches = new Dictionary<int, string>();
            batches = loadComboBoxBatch();
            var batchId = batches.FirstOrDefault(x => x.Value == student.batch_name).Key;
            MessageBox.Show("" + batchId);
            batch.SelectedValue = batchId;

            Dictionary<int, string> classes = new Dictionary<int, string>();
            classes = loadComboBoxClass();
            var classId = classes.FirstOrDefault(x => x.Value == student.class_name).Key;
            @class.SelectedValue = classId;

            Admission.SelectedDate = DateTime.ParseExact(student.admission_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            address.Text = student.address;
            if (student.gender == "Male")
            {
                Male.IsChecked = true;
            }
            else
            {
                Female.IsChecked = true;
            }
                disable();
        }
        private void Update()
        {
            @class.IsEnabled = true;
            contact.IsEnabled = true;
            Roll.IsEnabled = true;
            address.IsEnabled = true;
        }
        public void disable()
        {
            @class.IsEnabled = false;
            name.IsEnabled = false;
            contact.IsEnabled = false;
            Roll.IsEnabled = false;
            batch.IsEnabled = false;
            address.IsEnabled = false;
            Admission.IsEnabled = false;
            Male.IsEnabled = false;
            Female.IsEnabled = false;
        }
    }
}
