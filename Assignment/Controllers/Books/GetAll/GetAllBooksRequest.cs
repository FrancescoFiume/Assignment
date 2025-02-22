using Assignment.Db.Models;

namespace Assignment.Controllers.Books.GetAll;

public sealed class GetAllBooksRequest
{
    public string? Title { get; init; }
    public string? Author { get; init; }
    public Availability? Status { get; init; }
}