using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class DashboardVM
    {
        public int TotalUsers { get; set; }
        public int TotalBooks { get; set; }
        public int TotalOrders { get; set; }
        public int TotalRevenue { get; set; }

        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }

        public List<Order> RecentOrders { get; set; } = new List<Order>();
        public List<Book> LowStockBooks { get; set; } = new List<Book>();
    }
}
