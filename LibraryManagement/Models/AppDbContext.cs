using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }
        public DbSet<IssuedBook> IssuedBooks{ get; set; }
    }
}
