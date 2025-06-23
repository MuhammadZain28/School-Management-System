using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DL
{
    public interface INotificationStrategy
    {
        bool Send(InAppNotifications notifications);
        List<InAppNotifications> GetNotifications();
        List<InAppNotifications> GetReadNotifications();
        bool MarkAsRead(int userId);
    }

}
