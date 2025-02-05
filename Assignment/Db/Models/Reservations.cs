namespace Assignment.Data.Models;

public class Reservations:IObjectDb
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    
    public Books Book { get; set; }
    public Customers Customer { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Reservations reservation)
        {
            return false;
        }
        
        return reservation.CustomerId == CustomerId && 
               reservation.BookId == BookId && 
               reservation.ExpirationDate == ExpirationDate;
    }
}