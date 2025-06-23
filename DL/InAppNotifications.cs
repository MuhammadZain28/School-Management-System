using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;

namespace LMS.DL
{
    public class InAppNotifications
    {
        public int Notif_id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string message { get; set; }
        public string Date { get; set; }
        public int IsSeen {  get; set; }
    }
}
