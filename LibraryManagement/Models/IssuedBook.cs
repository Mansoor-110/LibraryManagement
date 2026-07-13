using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class IssuedBook
    {
        [Key]
        public int issuedBookId { get; set; }

        public int borrowRequestid { get; set; }
        [ForeignKey("borrowRequestid")]
        public virtual BorrowRequest BorrowRequest{ get; set; }
       
        public DateTime issuedDate { get; set; }= DateTime.Now;
        public DateTime returnDate { get; set; }
        public DateTime? actualReturnDate { get; set; }
        public int fineAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string status { get; set; } = "Issued";

    }
}
