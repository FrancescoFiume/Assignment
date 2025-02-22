using System.ComponentModel.DataAnnotations;
using Assignment.Attributes;

namespace Assignment.Controllers.Books.Update;

public sealed class UpdateBookRequest
{
    [Required]
    public string Title { get; init; } = string.Empty;
    
    [Required]
    public string Author { get; init; } = string.Empty;
    
    [Required, Isbn]
    public string Isbn { get; init; } = string.Empty;
}