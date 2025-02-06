using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment.Data.Models;

#pragma warning disable CS1591
public class Customers:IObjectDb
{
    public int Id { get; set; }
    
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    public virtual ICollection<Reservations> Reservations { get; set; }

    
}