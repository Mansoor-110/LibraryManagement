using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class IssuedBook
    {
        [Key]
        public int issuedBookId { get; set; }

        public int User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        public DateTime issuedDate { get; set; }
        public DateTime returnDate { get; set; }

        public int fineAmount { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string status { get; set; } = "Issued";

    }
}
