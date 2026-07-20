using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class LibrarianDashboardVM
    {
        public int TotalUsers { get; set; }
        public int TotalBooks { get; set; }

        public int TotalBorrowRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }

        public int TotalIssuedBooks { get; set; }
        public int OverdueBooks { get; set; }
        public int TotalOutstandingFine { get; set; }

        public List<BorrowRequestVM> RecentBorrowRequests { get; set; } = new List<BorrowRequestVM>();
        public List<IssuedBookVM> RecentIssuedBooks { get; set; } = new List<IssuedBookVM>();
        public List<Book> LowBorrowStockBooks { get; set; } = new List<Book>();
    }
}
