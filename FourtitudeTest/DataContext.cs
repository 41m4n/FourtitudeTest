using FourtitudeTest.Model.Dbm;
using Microsoft.EntityFrameworkCore;

namespace FourtitudeTest
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<PartnerDbm> partnerDbm { get; set; }
        public DbSet<ItemDetailDbm> itemDetailDbm { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Partner has many ItemDetails
            modelBuilder.Entity<PartnerDbm>()
                .HasMany(p => p.items)
                .WithOne(i => i.partner)
                .HasForeignKey(i => i.partnerDbmId)
                .OnDelete(DeleteBehavior.NoAction); // Optional: cascade delete

            base.OnModelCreating(modelBuilder);
        }
    }
}
