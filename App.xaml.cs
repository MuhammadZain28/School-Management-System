using System.Configuration;
using System.Data;
using System.Windows;
using OfficeOpenXml;
using Application = System.Windows.Application;

namespace LMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // ✅ Set EPPlus license once globally
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            base.OnStartup(e);
        }
    }

}
