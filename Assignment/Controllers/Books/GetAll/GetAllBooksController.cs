using Asp.Versioning;
using Assignment.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers.Books.GetAll;

[ApiController]
[Route("books")]
[ApiVersion("1.0")]
[Tags(Tags.Books)]
public sealed class GetAllBooksController : ControllerBase
{
    private readonly AppDbContext _db;

    public GetAllBooksController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Fetch all the Books
    /// </summary>
    /// <response code="200">Returned a list of all the books in the database</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet]
    [ProducesResponseType<GetAllBooksResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllBooksRequest request, CancellationToken cancellationToken)
    {
        var books = await _db.Books
            .Where(x =>
                (request.Author == null || x.Author.ToLower().Contains(request.Author.ToLower())) &&
                (request.Title == null || x.Title.ToLower().Contains(request.Title.ToLower())) &&
                (request.Status == null || x.Status == request.Status)
            )
            .Select(x => new GetAllBooksResponse.BookDto
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                Status = x.Status,
                Isbn = x.ISBN,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return Ok(new GetAllBooksResponse
        {
            Books = books,
        });
    }
}