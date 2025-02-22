using Asp.Versioning;
using Assignment.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers.Books.Delete;

[ApiController]
[Route("books/{id:int}")]
[ApiVersion("1.0")]
[Tags(Tags.Books)]
public sealed class DeleteBookController : ControllerBase
{
    private readonly AppDbContext _db;

    public DeleteBookController(AppDbContext db)
    {
        _db = db;
    }

    ///  <summary>
    ///  Delete one element from the Database
    ///  </summary>
    ///  <param name="id">
    /// Id is just the int of the element that you want to delete.
    ///  </param>
    ///  <param name="cancellationToken"></param>
    ///  <response code="204">Id deleted successfully</response>
    ///  <response code="400">Something went wrong</response>
    ///  <response code="404">Id not found</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var book = await _db.Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (book is null)
            return NotFound();
        
        _db.Books.Remove(book);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}