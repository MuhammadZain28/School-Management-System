using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;

namespace LMS.BL
{
    class Strategy : INotificationStrategy
    {
        private INotificationStrategy _strategy;

        public Strategy(INotificationStrategy strategy)
        {
            _strategy = strategy;
        }
        public void SetStrategy(INotificationStrategy strategy)
        {
            _strategy = strategy;
        }
        public bool Send(InAppNotifications notifications)
        {
            return _strategy.Send(notifications);
        }
        public List<InAppNotifications> GetNotifications()
        {
            return _strategy.GetNotifications();
        }
        public List<InAppNotifications> GetReadNotifications()
        {
            return _strategy.GetReadNotifications();
        }

        public bool MarkAsRead(int userId)
        {
            return _strategy.MarkAsRead(userId);
        }
    }
}
