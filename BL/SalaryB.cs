using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;
using OfficeOpenXml.Style;
using OfficeOpenXml;

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
        public void OverallSalryReport(int month)
        {

            int j = 0;
            List<BatchesB> Branches = BatchesD.BranchSalaries();
            Dictionary<string, DataTable> pairs = new Dictionary<string, DataTable>();

            foreach (var keyValue in Branches)
            {
                pairs.Add(
                    keyValue.BranchName, // branch name
                    SalaryD.GetSalaryReport(month, keyValue.branchId) // branch data
                );
            }

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel File (*.xlsx)|*.xlsx",
                FileName = $"Salary_Report_{DateTime.Now:yyyy_MM_dd}.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("SalaryReport");
                int currentRow = 1;

                foreach (var entry in pairs)
                {
                    string branchName = entry.Key;
                    DataTable dt = entry.Value;
                    int colCount = dt.Columns.Count;

                    // Merge header row
                    var mergedHeader = sheet.Cells[currentRow, 1, currentRow, colCount];
                    mergedHeader.Merge = true;
                    mergedHeader.Value = $"{branchName}";
                    mergedHeader.Style.Font.Bold = true;
                    mergedHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    currentRow++;

                    // Column headers
                    for (int i = 0; i < colCount; i++)
                    {
                        sheet.Cells[currentRow, i + 1].Value = dt.Columns[i].ColumnName;
                        sheet.Cells[currentRow, i + 1].Style.Font.Bold = true;
                        sheet.Cells[currentRow, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[currentRow, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }
                    currentRow++;

                    // Data rows
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < colCount; i++)
                        {
                            sheet.Cells[currentRow, i + 1].Value = row[i]?.ToString();
                        }
                        currentRow++;
                    }

                    sheet.Cells[currentRow, 1].Value = "Total :";
                    sheet.Cells[currentRow, 1].Style.Font.Bold = true;
                    sheet.Cells[currentRow, 1].Style.Fill.PatternType= ExcelFillStyle.Solid;
                    sheet.Cells[currentRow, 1].Style.Fill.BackgroundColor.SetColor (System.Drawing.Color.LightGray);

                    sheet.Cells[currentRow, 2].Value = Branches[j].Salaries;
                    sheet.Cells[currentRow, 2].Style.Font.Bold = true;
                    sheet.Cells[currentRow, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[currentRow, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    j++;
                    currentRow++; // Extra space between branches
                }

                // Auto fit columns
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                // Save file
                var file = new FileInfo(dialog.FileName);
                package.SaveAs(file);
            }

            MessageBox.Show("Excel file with all branch data was saved successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
