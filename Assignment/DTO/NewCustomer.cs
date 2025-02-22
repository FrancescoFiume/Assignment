using System.ComponentModel.DataAnnotations;

namespace Assignment.DTO;

#pragma warning disable CS1591
public struct NewCustomer
{
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}