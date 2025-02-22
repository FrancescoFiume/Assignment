using Asp.Versioning;
using Assignment.Db;
using Assignment.Db.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers.Books.UpdateStatus;

[ApiController]
[Route("books/{id:int}/status/{status}")]
[ApiVersion("1.0")]
[Tags(Tags.Books)]
public sealed class UpdateBookStatusController : ControllerBase
{
    private readonly AppDbContext _db;

    public UpdateBookStatusController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// This put request takes care alone of the available / unavailable switch
    /// </summary>
    /// <param name="id">book that is being reserved or given back </param>
    /// <param name="status"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Status switched to opposite successfully</response>
    /// <response code="400">Something went wrong</response>
    /// <response code="404">Id doesn't exist</response>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] int id,
        [FromRoute] Availability status,
        CancellationToken cancellationToken
    )
    {
        var book = await _db.Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (book is null)
            return NotFound();
        
        book.Status = status;
        
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}