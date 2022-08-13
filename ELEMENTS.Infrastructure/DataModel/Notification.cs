using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{


    public class FileNotification
    {
        public string FullFilePath { get; set; }
        public Guid FileGUID { get; set; }
        public string OriginalFileName { get; set; }
        public string FileTextContent { get; set; }
    }
    public class FileNotificationService
    {
        public event Func<FileNotification, Task> Notification;

        public void Notify(FileNotification notification)
        {
            if (Notification != null)
            {
                Notification.Invoke(notification);
            }
        }
    }



    public class InformationNotification
    {
        public string Title { get; set; } = string.Empty;
        public string Information { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
        public IDTO DTO { get; set; }
    }
    public class InformationNotificationService
    {
        public event Func<InformationNotification, Task> Notification;

        public void Notify(InformationNotification notification)
        {
            if (Notification != null)
            {
                Notification.Invoke(notification);
            }
        }
    }



}
