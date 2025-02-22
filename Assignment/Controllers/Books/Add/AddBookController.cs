using Asp.Versioning;
using Assignment.Db;
using Assignment.Db.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers.Books.Add;

[ApiController]
[Route("books")]
[ApiVersion("1.0")]
[Tags(Tags.Books)]
public sealed class AddBookController : ControllerBase
{
    private readonly AppDbContext _db;

    public AddBookController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Add new Book
    /// </summary>
    /// <param name="request">
    /// NewBook will be converted in a Struct, it needs to have:<br/>
    /// Title<br/>
    /// Author<br/>
    /// ISBN<br/>
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">Book successfully added</response>
    /// <response code="400">Something went wrong</response>
    [HttpPost]
    [ProducesResponseType<AddBookResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] AddBookRequest request, CancellationToken cancellationToken)
    {
        var book = new BookDbo
        {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.Isbn,
        };
        
        _db.Books.Add(book);
        await _db.SaveChangesAsync(cancellationToken);
        
        return Created($"api/v1/Book/{book.Id}", new AddBookResponse
        {
            Id = book.Id,
            Title = book.Title,
        });
    }
}