using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment.Data.Models;

public class Customers:IObjectDb
{
    public int Id { get; set; }
    
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    public virtual ICollection<Reservations> Reservations { get; set; }


    /// <summary>
    /// Overriding Equals is useful to understand if the underlying class had any changes before triggering
    /// Update
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>returns true if objects are equal false if are different</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Customers customers)
        {
            return false;
        }
        
        return customers.FirstName == FirstName && 
               customers.LastName == LastName && 
               customers.Email == Email;
    }
}