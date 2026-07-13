namespace LibraryManagement.Models
{
    public class IssuedBookVM
    {
        public int issuedBookId { get; set; }
        public string BookName { get; set; }
        public string BookImageName { get; set; }
        public string User_name { get; set; }
        public string Email { get; set; }
        public DateTime issuedDate { get; set; } 
        public DateTime returnDate { get; set; }
        public int fineAmount { get; set; }
        public string status { get; set; } 
    }
}
