using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int User_id { get; set; }
        public virtual User User { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public int Quantity { get; set; } = 1;

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
