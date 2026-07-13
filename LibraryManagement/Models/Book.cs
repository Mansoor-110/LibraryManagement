using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string BookName { get; set; }
        [Required]
        [Column(TypeName = "varchar(500)")]
        public string BookDescription { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string BookAuthor { get; set; }
        [Required]
       
        public int BookPages { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string BookLanguage { get; set; }
        [Required]
       
        public int quantity { get; set; }
        [Required]
       
        public int qtyforborrow { get; set; }
        [Required]
        
        public int BookPrice { get; set; }

        [NotMapped]
        [Required]
        public IFormFile BookImage { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string BookImageName { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<BorrowRequest> BorrowRequests { get; set; }
        public virtual ICollection<IssuedBook> IssuedBooks { get; set; }

    }
}
