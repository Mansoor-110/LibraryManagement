namespace LibraryManagement.Models
{
    public class CartItemVM
    {
        public int CartItemId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookImageName { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
