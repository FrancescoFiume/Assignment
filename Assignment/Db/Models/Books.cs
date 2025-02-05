using System.Reflection;

namespace Assignment.Data.Models;

public class Books
{
    public required int Id { get; set; } 
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string ISBN { get; set; }
    public required int Available { get; set; }
    public required Availability Status { get; set; }
    public enum Availability : ushort
    {
        Unavailable=0,
        Available=1   
    }
    
    public virtual ICollection<Reservations> Reservations { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Books book)
        {
            return false;
        }
        
        return book.Title == Title && 
               book.Author == Author && 
               book.ISBN == ISBN;
    }
}