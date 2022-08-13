using ePine.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ePine.DataAccess.Connections;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<Merchant?> Merchants { get; set; }
    public DbSet<Appointment?> Appointments { get; set; }
    // public DbSet<MerchantAdministrator?> MerchantAdministrators{ get; set; }
    //
    // void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<MerchantAdministrator>()
    //         .HasOne<Merchant>()
    //         ;
    // }
}
