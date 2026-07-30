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
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<OrderItem> OrderItems{ get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. CartItem -> User (Cascade delete for User)
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.User_id)
                .OnDelete(DeleteBehavior.Cascade); // Change to Cascade

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. BorrowRequest -> User (Cascade delete for User)
            modelBuilder.Entity<BorrowRequest>()
                .HasOne(b => b.User)
                .WithMany(u => u.BorrowRequests)
                .HasForeignKey(b => b.User_id)
                .OnDelete(DeleteBehavior.Cascade); // Change to Cascade

            modelBuilder.Entity<BorrowRequest>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.BorrowRequests)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. IssuedBook -> BorrowRequest
            modelBuilder.Entity<IssuedBook>()
                .HasOne(i => i.BorrowRequest)
                .WithMany(br => br.IssuedBooks)
                .OnDelete(DeleteBehavior.Cascade); // BorrowRequest ke saath IssuedBook bhi udd jaye
        }
    }
    
}
