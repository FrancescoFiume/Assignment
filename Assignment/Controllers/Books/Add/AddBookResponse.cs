namespace Assignment.Controllers.Books.Add;

public sealed class AddBookResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; } = string.Empty;
}