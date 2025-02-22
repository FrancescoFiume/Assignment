using Asp.Versioning;
using Assignment.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers.Books.Get;

[ApiController]
[Route("books/{id:int}")]
[ApiVersion("1.0")]
[Tags(Tags.Books)]
public sealed class GetBookController : ControllerBase
{
    private readonly AppDbContext _db;

    public GetBookController(AppDbContext db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Fetch all the Books
    /// </summary>
    /// <response code="200">Returned a list of all the books in the database</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet]
    [ProducesResponseType<GetBookResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var book = await _db.Books
            .Select(x => new GetBookResponse
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                Status = x.Status,
                Isbn = x.ISBN,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return Ok(book);
    }
}