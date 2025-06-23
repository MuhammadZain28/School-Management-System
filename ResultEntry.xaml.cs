using System;
using System.Collections.Generic;
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
using Google.Protobuf.WellKnownTypes;
using LMS.BL;
using LMS.DL;
using MySqlX.XDevAPI.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for ResultEntry.xaml
    /// </summary>
    public partial class ResultEntry : Page
    {
        private int classid {  get; set; }
        private int exam {  get; set; }
        private int resultId { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSaved = false;
        public ResultEntry(int Examid = -1, int classid = -1, int resultId = -1, bool isEdit = false )
        {
            InitializeComponent();
            this.classid = classid;
            this.exam = Examid;
            this.resultId = resultId; 
            IsUpdate = isEdit;
            load();
            if (resultId > 0)
                if (IsUpdate)
                    Update();
                else
                    View();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }
        public void AddData()
        {
            if (!IsUpdate)
            {
                ResultB result = inputData();
                if (result.AddResult(result))
                    MessageBox.Show("Success");
                else
                    MessageBox.Show("UnSuccessful");
            }
            else
            {
                ResultB result = inputData();
                if (result.UpdateResult(result, resultId))
                    MessageBox.Show("Success");
                else
                    MessageBox.Show("UnSuccessful");
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.No;
            if (!IsSaved)
            {
                result = MessageBox.Show("Want to save data", "Unsaved Work", MessageBoxButton.YesNo);
            }
            if (result == MessageBoxResult.Yes)
            {
                AddData();
            }
            MainFrame.Navigate(new Result());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void load()
        {
            StudentB studentB = new StudentB();
            roll.ItemsSource = studentB.IdRollPairs(classid);
            roll.DisplayMemberPath = "Value";
            roll.SelectedValuePath = "Key";
            grade.Items.Add("A+");
            grade.Items.Add("A");
            grade.Items.Add("B+");
            grade.Items.Add("B");
            grade.Items.Add("C");
            grade.Items.Add("D");
            grade.Items.Add("E");
            grade.Items.Add("F");
        }

        private ResultB inputData()
        {
            ResultB result = new ResultB();

            int obtainedMarks;
            if (int.TryParse(marks.Text, out obtainedMarks))
            {
                result.ObtainedMarks = obtainedMarks;
                result.Grade = grade.Text; 
                TextRange textRange = new TextRange(remarks.Document.ContentStart, remarks.Document.ContentEnd);
                result.Remarks = textRange.Text;
                result.Students.roll = roll.Text;
                result.Exams.ExamId = exam;
            }
            else
            {
                MessageBox.Show("Invalid input for Obtained Marks.");
            }

            return result;
        }
        private void disable()
        {
            marks.IsEnabled = false;
            roll.IsEnabled = false;
            grade.IsEnabled = false;
            remarks.IsEnabled = false;
            Submit.IsEnabled = false;
        }
        private void View()
        {
            ResultB result = new ResultB();
            result = result.SearchResult(resultId); 
            roll.SelectedItem = result.Students.roll;

            grade.SelectedItem = result.Grade;
            marks.Text = result.ObtainedMarks.ToString();
            exam = result.Exams.ExamId;

            remarks.Document.Blocks.Clear();
            remarks.Document.Blocks.Add(new Paragraph(new Run(result.Remarks)));
            disable();
        }
        private void Update()
        {
            View();
            grade.IsEnabled = true;
            remarks.IsEnabled= true;
            marks.IsEnabled= true;
            Submit.IsEnabled= true;
        }
    }
}
