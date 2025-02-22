using Assignment.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Customers> Customers { get; set; }
    public virtual DbSet<BookDbo> Books { get; set; }
    public virtual DbSet<Reservations> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        // builder.Properties<Enum>().HaveConversion<string>();
    }
}