using Assignment.Db.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers.Books.Get;


public sealed class GetBookResponse : ControllerBase
{
    public required int Id { get; init; }
    public required string Title { get; init; } = string.Empty;
    public required string Author { get; init; }
    public required Availability Status { get; init; }
    public required string Isbn { get; init; }
}