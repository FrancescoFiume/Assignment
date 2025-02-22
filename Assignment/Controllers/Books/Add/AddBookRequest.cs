using System.ComponentModel.DataAnnotations;
using Assignment.Attributes;

namespace Assignment.Controllers.Books.Add;

public sealed class AddBookRequest
{
    [Required]
    public string Title { get; init; } = string.Empty;
    
    [Required]
    public string Author { get; init; } = string.Empty;
    
    [Isbn]
    [Required]
    public string Isbn { get; init; } = string.Empty;
}