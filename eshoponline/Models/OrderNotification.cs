using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Models
{
    public enum OrderNotificationType
    {
        Processed,
        QualityCheckPassed,
        Dispatched,
        Delivered,
        Canceled,
        Return
    }

    public class OrderNotification
    {
        public int OrderNotificationId { get; set; }
        public OrderNotificationType Type { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }
}
