using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Assignment.Db.Models;

public class Customers : IObjectDb, IEntityTypeConfiguration<Customers>
{
    public int Id { get; set; }


    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime RegistrationDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Reservations> Reservations { get; set; }

    public void Configure(EntityTypeBuilder<Customers> builder)
    {
        //Customer Region
        //PK and Table Definition
        builder
            .ToTable("Customer")
            .HasKey(customer => customer.Id);

        //Id
        builder
            .Property(customer => customer.Id)
            .HasColumnName("Id")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
            .ValueGeneratedOnAdd()
            .IsRequired();
        //FirstName
        builder
            .Property(customer => customer.FirstName)
            .HasColumnName("FirstName")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //LastName
        builder
            .Property(customer => customer.LastName)
            .HasColumnName("LastName")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //Email
        builder
            .Property(customer => customer.Email)
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128);
        //RegistrationDate
        builder
            .Property(customer => customer.RegistrationDate)
            .HasColumnName("RegistrationDate")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }
}