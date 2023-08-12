using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingDAL
{
     public class BugContext : IdentityDbContext<GeneralUser>
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Tester> Testers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public BugContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            base.OnModelCreating(builder);
        }
    }
}
