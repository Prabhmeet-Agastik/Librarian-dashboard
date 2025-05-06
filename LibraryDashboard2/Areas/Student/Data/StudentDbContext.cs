


using Microsoft.EntityFrameworkCore;
using LibraryDashboard2.Areas.Student.Models;



namespace LibraryDashboard2.Areas.Student.Data

{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }
         
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<IssuedBook> IssuedBooks { get; set; }

        public DbSet<ReturnBookRequest> ReturnBookRequests { get; set; }

    }
}
