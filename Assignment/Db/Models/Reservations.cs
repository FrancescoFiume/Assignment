using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Assignment.Db.Models;

#pragma warning disable CS1591
public class Reservations : IObjectDb, IEntityTypeConfiguration<Reservations>
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ExpirationDate { get; set; }

    public BookDbo BookDbo { get; set; }
    public Customers Customer { get; set; }

    public void Configure(EntityTypeBuilder<Reservations> builder)
    {
        
        //Reservation Region
        //Table and PK mapping
        builder
            .ToTable("Reservation")
            .HasKey(reservation => reservation.Id);
        //Id
        builder
            .Property(reservation => reservation.Id)
            .HasColumnName("Id")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
            .ValueGeneratedOnAdd()
            .IsRequired();
        //Books foreign key
        builder
            .HasOne(reservation => reservation.BookDbo)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        //Customer foreign keyd
        builder
            .HasOne(reservation => reservation.Customer)
            .WithMany(customer => customer.Reservations)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        //Reservation Date
        builder
            .Property(reservation => reservation.ReservationDate)
            .HasColumnName("ReservationDate")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        //Reservation Expiration Date, 15 days after the reservation
        builder
            .Property(reservation => reservation.ExpirationDate)
            .HasColumnName("ExpirationDate")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP + INTERVAL '15 days'");
    }
}