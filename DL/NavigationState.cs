using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DL
{
    class NavigationState
    {
        public static bool HasUnsavedChanges { get; set; } = false;
        public static bool SelectSame { get; set; } = false;
        public static string timespan { get; set; } = "0";
    }
}
