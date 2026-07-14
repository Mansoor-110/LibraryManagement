namespace LibraryManagement.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }

        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookImageName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public Order Order { get; set; }
    }
}
