using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class CheckoutVM
    {
        public List<CartItemVM> CartItems { get; set; }
        public int TotalAmount { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
    }
}
