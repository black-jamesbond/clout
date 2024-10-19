using Microsoft.EntityFrameworkCore;
using clout.Entity;

namespace clout.DbContexts
{
    public class clout_DbContexts : DbContext
    {
        public clout_DbContexts(DbContextOptions<clout_DbContexts> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
