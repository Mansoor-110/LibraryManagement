namespace LibraryManagement.Models
{
    public class BorrowRequestVM
    {
        public int borrowRequestid { get; set; }
        public string BookName { get; set; }
        public string BookImageName { get; set; }
        public int qtyforborrow { get; set; }
        public string User_name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
