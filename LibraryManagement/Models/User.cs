using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class User
    {
        [Key]
        public int User_id { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string User_name {get; set;}
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "varchar(255)")]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<BorrowRequest> BorrowRequests { get; set; }
        public virtual ICollection<IssuedBook> IssuedBooks { get; set; }


    }
}
