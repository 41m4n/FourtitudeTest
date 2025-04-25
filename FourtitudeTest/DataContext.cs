using FourtitudeTest.Model.Dbm;
using Microsoft.EntityFrameworkCore;

namespace FourtitudeTest
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<PartnerDbm> partnerDbm { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
