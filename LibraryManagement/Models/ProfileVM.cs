namespace LibraryManagement.Models
{
    public class ProfileVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime MemberSince { get; set; }

        public int OutstandingFine { get; set; }
        public int BooksCurrentlyIssuedCount { get; set; }
        public int PendingRequestsCount { get; set; }

        public List<IssuedBookCardVM> IssuedBooks { get; set; }
        public List<BorrowRequestRowVM> BorrowRequests { get; set; }
        public List<HistoryRowVM> History { get; set; }
    }

    public class IssuedBookCardVM
    {
        public int IssuedBookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookImageName { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime DueDate { get; set; }
        public int CurrentFine { get; set; }

        public bool IsOverdue => DateTime.Now.Date > DueDate.Date;
        public int DaysOverdue => IsOverdue ? (DateTime.Now.Date - DueDate.Date).Days : 0;
    }

    public class BorrowRequestRowVM
    {
        public int BorrowRequestId { get; set; }
        public string BookName { get; set; }
        public DateTime RequestedOn { get; set; }
        public string Status { get; set; }
    }

    public class HistoryRowVM
    {
        public string BookName { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int FinePaid { get; set; }
        public bool ReturnedLate => FinePaid > 0;
    }
}