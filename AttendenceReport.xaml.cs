using System;
using System.Windows;
using System.Windows.Controls;
using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using System.IO;
using LMS.DL;
using LMS.BL;
using WebBrowser = System.Windows.Controls.WebBrowser;
using MessageBox = System.Windows.MessageBox;
using System.Data;

namespace LMS
{
    public partial class AttendenceReport : Page
    {
        public int Choice {  get; set; } 
        public AttendenceReport(int choice)
        {
            InitializeComponent();
            Choice = choice;
            if (choice == 0 || choice == 4)
                loadMonth();
            else
            {
                select.Content = "Year";
                loadYear();
            }
        }

        private Dictionary<int, string> loadMonth()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                months.Add(i, monthName);
            }

            month.ItemsSource = months;
            month.DisplayMemberPath = "Value";
            month.SelectedValuePath = "Key";
            return months;
        }

        private void loadYear()
        {
            for (int i = 2025; i <= DateTime.Now.Year; i++)
            {
                month.Items.Add(i);
            }
        }

        private void Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Choice == 0)
            {
                var selectedMonth = month.SelectedIndex + 1;
                try
                {
                    DataTable table = new DataTable("Attendence");

                    AttendenceB attendence = new AttendenceB();

                    table = attendence.GetDataTable(selectedMonth);

                    Report report = new Report();

                    report.RegisterData(table, "Attendence");
                    report.GetDataSource("Attendence").Enabled = true;
                    report.Load("FinanceReport.frx");

                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ReportOutput.pdf");
                    report.Export(new PDFSimpleExport(), pdfFilePath);

                    WebBrowser pdfViewer = new WebBrowser();
                    pdfViewer.Navigate(pdfFilePath);
                    Content = pdfViewer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }
            else if (Choice == 1)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                try
                {
                    DataTable table = new DataTable("Result");

                    ResultB resultB = new ResultB();

                    table = resultB.GetDataTable(selectedMonth);

                    Report report = new Report();

                    report.RegisterData(table, "Result");
                    report.GetDataSource("Result").Enabled = true;
                    report.Load("ResultReport.frx");

                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ResultOutput.pdf");
                    report.Export(new PDFSimpleExport(), pdfFilePath);

                    WebBrowser pdfViewer = new WebBrowser();
                    pdfViewer.Navigate(pdfFilePath);
                    Content = pdfViewer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }

            else if (Choice == 2)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                try
                {
                    DataTable table = new DataTable("Finance");

                    FeeB financeB = new FeeB();

                    table = financeB.GetDataTableMonth(selectedMonth);

                    Report report = new Report();

                    report.RegisterData(table, "Finance");
                    report.GetDataSource("Finance").Enabled = true;
                    report.Load("AttendaceReport.frx");

                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FinanceOutput.pdf");
                    report.Export(new PDFSimpleExport(), pdfFilePath);

                    WebBrowser pdfViewer = new WebBrowser();
                    pdfViewer.Navigate(pdfFilePath);
                    Content = pdfViewer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }
            else if (Choice == 3)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                try
                {
                    DataTable table = new DataTable("Teacher");

                    TeacherB teacherB = new TeacherB();

                    table = teacherB.GetDataTable();

                    Report report = new Report();

                    report.RegisterData(table, "Teacher");
                    report.GetDataSource("Teacher").Enabled = true;
                    report.Load("TeacherReport.frx");

                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TeacherOutput.pdf");
                    report.Export(new PDFSimpleExport(), pdfFilePath);

                    WebBrowser pdfViewer = new WebBrowser();
                    pdfViewer.Navigate(pdfFilePath);
                    Content = pdfViewer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }
            else if (Choice == 4)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                try
                {
                    DataTable table = new DataTable("Finance");

                    FeeB financeB = new FeeB();

                    table = financeB.GetDataTableMonth(selectedMonth);

                    Report report = new Report();

                    report.RegisterData(table, "Finance");
                    report.GetDataSource("Finance").Enabled = true;
                    report.Load("MonthlyReport.frx");

                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MonthlyFinanceOutput.pdf");
                    report.Export(new PDFSimpleExport(), pdfFilePath);

                    WebBrowser pdfViewer = new WebBrowser();
                    pdfViewer.Navigate(pdfFilePath);
                    Content = pdfViewer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }
        }
    }
}
