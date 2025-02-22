using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Assignment.Db.Models;

public sealed class BookDbo : IObjectDb, IEntityTypeConfiguration<BookDbo>
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }

    public required string ISBN { get; set; }
    public Availability Status { get; set; }

    [JsonIgnore]
    public ICollection<Reservations> Reservations { get; set; }


    public void Configure(EntityTypeBuilder<BookDbo> builder)
    {
        //Books Region
        //PK and Table Definition
        builder
            .ToTable("Book")
            .HasKey(book => book.Id);
        
        //Id
        builder
            .Property(book => book.Id)
            .HasColumnName("Id")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
            .ValueGeneratedOnAdd()
            .IsRequired();
        //Title
        builder
            .Property(book => book.Title)
            .HasColumnName("Title")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //Author
        builder
            .Property(book => book.Author)
            .HasColumnName("Author")
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        //ISBN
        builder
            .Property(book => book.ISBN)
            .HasColumnName("ISBN")
            .HasColumnType("VARCHAR")
            .HasMaxLength(16)
            .IsRequired();

        //Availability
        builder
            .Property(b => b.Status)
            .HasColumnName("Availability")
            .HasColumnType("INT")
            .HasDefaultValue(Availability.Available);
    }
}