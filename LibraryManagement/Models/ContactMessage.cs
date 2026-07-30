using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class ContactMessage
    {
        [Key]
        public int ContactMessageId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Subject { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public bool IsResolved { get; set; } = false;
    }
}