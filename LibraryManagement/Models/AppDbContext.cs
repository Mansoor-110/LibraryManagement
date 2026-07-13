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
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IssuedBook -> BorrowRequest
            modelBuilder.Entity<IssuedBook>()
                .HasOne(i => i.BorrowRequest)
                .WithMany(br => br.IssuedBooks)    
                .OnDelete(DeleteBehavior.Restrict);

            // BorrowRequest -> User
            modelBuilder.Entity<BorrowRequest>()
                .HasOne(b => b.User)
                .WithMany(u => u.BorrowRequests)    
                .OnDelete(DeleteBehavior.Restrict);

            // BorrowRequest -> Book
            modelBuilder.Entity<BorrowRequest>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.BorrowRequests)  // ← actual collection name diya
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.User_id)      // ← ye line add ki
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId)       // ← ye line add ki
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    
}
