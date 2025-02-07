namespace Assignment.Data.Models;

#pragma warning disable CS1591
public class Reservations : IObjectDb
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ExpirationDate { get; set; }

    public Books Book { get; set; }
    public Customers Customer { get; set; }

}