using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<BookTableMVC> Book { get; set; }
    }
}
