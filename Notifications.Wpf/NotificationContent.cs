using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Wpf
{
    public class NotificationContent
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string con { get; set; }
        public string msg_type { get; set; }
        public string image { get; set; }
        public string link{ get; set; }
        public DateTime time { get; set; }
        public string time_view { get; set; }

        public NotificationType Type { get; set; }
    }

    public enum NotificationType   
    {
        Information,
        Success,
        Warning,
        Error
    }
}
