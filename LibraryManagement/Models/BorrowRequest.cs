using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class BorrowRequest
    {
        [Key]
        public int borrowRequestid { get; set; }

        public int User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
