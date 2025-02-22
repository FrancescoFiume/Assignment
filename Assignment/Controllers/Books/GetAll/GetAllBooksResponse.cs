using Assignment.Db.Models;

namespace Assignment.Controllers.Books.GetAll;

public sealed class GetAllBooksResponse
{
    public required IEnumerable<BookDto> Books { get; init; }
    
    public sealed class BookDto
    {
        public required int Id { get; init; }
        public required string Title { get; init; } = string.Empty;
        public required string Author { get; init; }
        public required Availability Status { get; init; }
        public required string Isbn { get; init; }
    }
}

