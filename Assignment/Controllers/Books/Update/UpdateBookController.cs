using Asp.Versioning;
using Assignment.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers.Books.Update;

[ApiController]
[Route("books/{id:int}")]
[ApiVersion("1.0")]
[Tags(Tags.Books)]
public sealed class UpdateBookController : ControllerBase
{
    private readonly AppDbContext _db;

    public UpdateBookController(AppDbContext db)
    {
        _db = db;
    }


    /// <summary>
    /// Change one or more properties on the underlying object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request">
    /// This item must contain the Id and
    /// the string/strings that you want to modify.<br/>
    /// If you only want to change one field just send the Id<br/>
    /// and the string you want to change
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Update was successfull</response>
    /// <response code="400">Something went wrong</response>
    /// <response code="404">Id doesn't exist</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateBookRequest request,
        CancellationToken cancellationToken
    )
    {
        var book = await _db.Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (book is null)
            return NotFound();

        var isbnExists = await _db.Books
            .Where(x => x.ISBN == request.Isbn)
            .Where(x => x.Id != id)
            .AnyAsync(cancellationToken);
        
        if (isbnExists)
            ModelState.AddModelError(nameof(request.Isbn), "ISBN already exists");

        if (!ModelState.IsValid)
            return ValidationProblem();
        
        book.Title = request.Title;
        book.Author = request.Author;
        book.ISBN = request.Isbn;
        
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}