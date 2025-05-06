using Microsoft.EntityFrameworkCore;
using LibraryDashboard2.Models;



namespace LibraryDashboard2.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<IssuedBook> IssuedBooks { get; set; }
        public DbSet<ReturnBookRequest> ReturnBookRequests { get; set; }

    }
}
