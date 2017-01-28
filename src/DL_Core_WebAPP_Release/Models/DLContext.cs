using Microsoft.EntityFrameworkCore;

namespace DL_Core_WebAPP_Release.Models
{
    public class DLContext : DbContext
    {
        public DLContext(DbContextOptions<DLContext> options)
            : base(options)
        { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Issue> Issues { get; set; }
    }
}


