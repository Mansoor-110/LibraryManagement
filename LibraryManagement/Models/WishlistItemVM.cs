namespace LibraryManagement.Models
{
    public class WishlistItemVM
    {
        public int WishlistItemId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookImageName { get; set; }
        public int BookPrice { get; set; }
        public int Quantity { get; set; } 
        public DateTime AddedAt { get; set; }
    }
}
