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
using FastReport.Utils;

namespace LMS
{
    public partial class AttendenceReport : Page
    {
        public int Choice {  get; set; } 
        public AttendenceReport(int choice)
        {
            InitializeComponent();
            Choice = choice;
            loadBatch();
            if (choice == 0 || choice == 1 || choice == 2 || choice == 3 || choice == 4 || choice == 5)
                loadMonth();
            else
            {
                select.Content = "Year";
                loadYear(); 
            }
            if (choice != 4 && choice != 5)
            {
                branch.Opacity = 0;
                branch.IsEnabled = false;
                br.Opacity = 0;
                br.IsEnabled = false;
                brr.Opacity = 0;
                brr.IsEnabled = false;
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
        private void loadBatch()
        {
            BatchesB batchesB = new BatchesB();
            br.ItemsSource = batchesB.loadBatch();
            br.DisplayMemberPath = "Value";
            br.SelectedValuePath = "Key";
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

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
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

                    string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "FinanceReport.frx");

                    if (File.Exists(reportPath))
                    {
                        report.Load(reportPath);
                    }
                    else
                    {
                        MessageBox.Show("Report file not found: " + reportPath);
                    }

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
                    DataTable table = new DataTable("Finance");

                    FeeB financeB = new FeeB();

                    table = financeB.GetDataTableMonth(selectedMonth);

                    Report report = new Report();

                    report.RegisterData(table, "Finance");
                    report.GetDataSource("Finance").Enabled = true;


                    string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "YearlyFinance.frx");

                    if (File.Exists(reportPath))
                    {
                        report.Load(reportPath);
                    }
                    else
                    {
                        MessageBox.Show("Report file not found: " + reportPath);
                    }


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

            else if (Choice == 2)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                try
                {
                    DataTable Branch = new DataTable("Branch");
                    Branch = BatchesD.BranchSalariesTable();

                    DataTable teacher = new DataTable("Teachers");
                    teacher = SalaryD.GetOverallSalary(selectedMonth);

                    Report report = new Report();


                    report.RegisterData(Branch, "Branch");
                    report.GetDataSource("Branch").Enabled = true;

                    report.RegisterData(teacher, "Teachers");
                    report.GetDataSource("Teachers").Enabled = true;

                    string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "SalaryReport.frx");
                    if (File.Exists(reportPath))
                    {
                        report.Load(reportPath);
                    }
                    else
                    {
                        MessageBox.Show("Report not Found at "+reportPath,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SalaryReport.pdf");
                    report.Export(new PDFSimpleExport(), pdfFilePath);

                    WebBrowser webBrowser = new WebBrowser();
                    webBrowser.Navigate(new Uri(pdfFilePath));
                    Content = webBrowser;
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
                    SalaryB salary = new SalaryB();

                    salary.OverallSalryReport(selectedMonth);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }
            else if (Choice == 4)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                int selectedBatch = Convert.ToInt32(br.SelectedValue.ToString());
                TeacherAttendenceB teacher = new TeacherAttendenceB();
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV file (*.csv)|*.csv",
                    FileName = $"Attendance_Report_{DateTime.Now:yyyy_MM_dd}.csv"
                };

                if (dialog.ShowDialog() == true)
                {
                    teacher.ExportAttendanceToCSV(dialog.FileName, selectedMonth, selectedBatch);
                }
            }

            else if (Choice == 5)
            {
                int selectedMonth = Convert.ToInt32(month.SelectedValue.ToString());
                int selectedBatch = Convert.ToInt32(br.SelectedValue.ToString());

                try
                {
                    DataTable table = new DataTable("Teacher_attendence");


                    table = SalaryD.GetSalaryReport(selectedMonth, selectedBatch);

                    Report report = new Report();
                    
                    report.RegisterData(table, "Teacher_attendence");


                    report.GetDataSource("Teacher_attendence").Enabled = true;

                    string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "TeacherAttendenceReport.frx");

                    if (File.Exists(reportPath))
                    {
                        report.Load(reportPath);
                    }
                    else
                    {
                        MessageBox.Show("Report file not found: " + reportPath);
                    }

                    report.Prepare();

                    string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TeacherSalaryOutput.pdf");
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
