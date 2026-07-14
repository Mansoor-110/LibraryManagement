namespace LibraryManagement.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int User_id { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string PaymentMethod { get; set; } = "Cash on Delivery";
        public int TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
