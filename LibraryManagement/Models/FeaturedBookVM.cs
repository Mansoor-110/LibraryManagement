namespace LibraryManagement.Models
{
    public class FeaturedBookVM
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookImageName { get; set; }
        public int BookPrice { get; set; }
        public int BookPages { get; set; }
        public string BookLanguage { get; set; }
        public int Quantity { get; set; }
    }
}