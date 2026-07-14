namespace LibraryManagement.Models
{
    public class WishlistItem
    {
        public int WishlistItemId { get; set; }
        public int User_id { get; set; }
        public int BookId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;

        public Book Book { get; set; }
    }
}
