using Assignment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Assignment.Data;
#pragma warning disable CS1591
public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}

    public virtual DbSet<Customers> Customers { get; set; }
    public virtual DbSet<Books> Books { get; set; }
    public virtual DbSet<Reservations> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Customer Region
        //PK and Table Definition
        modelBuilder.Entity<Customers>()
            .ToTable("Customer")
            .HasKey(customer => customer.Id);
            
        //Id
        modelBuilder.Entity<Customers>()
            .Property(customer => customer.Id)
            .HasColumnName("Id")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
            .ValueGeneratedOnAdd()
            .IsRequired();
        //FirstName
        modelBuilder.Entity<Customers>()
            .Property(customer=> customer.FirstName)
            .HasColumnName("FirstName")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //LastName
        modelBuilder.Entity<Customers>()
            .Property(customer => customer.LastName)
            .HasColumnName("LastName")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //Email
        modelBuilder.Entity<Customers>()
            .Property(customer => customer.Email)
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128);
        //RegistrationDate
        modelBuilder.Entity<Customers>()
            .Property(customer=> customer.RegistrationDate)
            .HasColumnName("RegistrationDate")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        //Books Region
        //PK and Table Definition
        modelBuilder.Entity<Books>()
            .ToTable("Book")
            .HasKey(book => book.Id);
        //Id
        modelBuilder.Entity<Books>()
            .Property(book => book.Id)
            .HasColumnName("Id")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
            .ValueGeneratedOnAdd()
            .IsRequired();
        //Title
        modelBuilder.Entity<Books>()
            .Property(book=> book.Title)
            .HasColumnName("Title")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //Author
        modelBuilder.Entity<Books>()
            .Property(book=> book.Author)
            .HasColumnName("Author")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //ISBN
        modelBuilder.Entity<Books>()
            .Property(book=> book.ISBN)
            .HasColumnName("ISBN")
            .HasColumnType("VARCHAR")
            .HasMaxLength(16)
            .IsRequired();

        //Enum Mapping
        modelBuilder.HasPostgresEnum<Books.Availability>();
        //Availability
        modelBuilder.Entity<Books>()
            .Property(b => b.Status)
            .HasColumnName("Availability")
            .HasColumnType("INT")
            .HasDefaultValue(Models.Books.Availability.Available);
        //Reservation Region
        //Table and PK mapping
        modelBuilder.Entity<Reservations>()
            .ToTable("Reservation")
            .HasKey(reservation => reservation.Id);
        //Id
        modelBuilder.Entity<Reservations>()
            .Property(reservation => reservation.Id)
            .HasColumnName("Id")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
            .ValueGeneratedOnAdd()
            .IsRequired();
        //Books foreign key
        modelBuilder.Entity<Reservations>()
            .HasOne(reservation => reservation.Book)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        //Customer foreign keyd
        modelBuilder.Entity<Reservations>()
            .HasOne(reservation => reservation.Customer)
            .WithMany(customer => customer.Reservations)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        //Reservation Date
        modelBuilder.Entity<Reservations>()
            .Property(reservation => reservation.ReservationDate)
            .HasColumnName("ReservationDate")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        //Reservation Expiration Date, 15 days after the reservation
        modelBuilder.Entity<Reservations>()
            .Property(reservation => reservation.ExpirationDate)
            .HasColumnName("ExpirationDate")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP + INTERVAL '15 days'");





    }
    
}