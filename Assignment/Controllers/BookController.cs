using System.Reflection;
using Assignment.CustomExceptions;
using Assignment.Data.Collections;
using Assignment.Data.Models;
using Assignment.DataCheck;
using Assignment.DataCheck.Checks.BookChecks;
using Assignment.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;

/// <summary>
/// Contains CRUD operations for book
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class BookController: ControllerBase
{
    private readonly BookCollection _bookCollection;
    private readonly ILogger<CustomerController> _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bookCollection">Dependency Injected books Collection</param>
    /// <param name="logger">Dependency Injected logger</param>
    public BookController(BookCollection bookCollection, ILogger<CustomerController> logger)
    {
        _logger = logger;
        _bookCollection = bookCollection;
    }
    /// <summary>
    /// Fetch all the Books
    /// </summary>
    /// <response code="200">Returned a list of all the books in the database</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet]
    public IActionResult All()
    {
        try
        {
            return Ok(_bookCollection);

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Fetch specific book by Id, if the book doesn't exist returns NotFound
    /// </summary>
    /// <param name="id">Id must be an integer</param>
    /// <response code="200">returns the requested book</response>
    /// <response code="404">Id doesn't exist</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet("{id}")]
    public  IActionResult GetOne([FromRoute]int id)
    {
        try
        {
            var book = _bookCollection.GetById(id);
            return Ok(book);
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {

            _logger.LogError("Id not found");
            return NotFound("Id not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// This function takes a string from the query body (Whatever is placed after title= or author= or both<br/>
    /// and looks in the Db if there is anything matching those parameters.
    /// </summary>
    /// <param name="title">nullable string for the title</param>
    /// <param name="author">nullable string for the author</param>
    /// <response code="200">returns data that contain the requested string (title or author or both)</response>
    /// <response code="404">Nothing matched the query</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet("search")]
    public IActionResult GetByQuery([FromQuery] string? title, [FromQuery] string? author)
    {

        try
        {
            List<Books> books;
            if (author is null && title is not null)
            {
                books = _bookCollection.Where(b =>
                    b.Title.Equals(title, StringComparison.OrdinalIgnoreCase) ||
                    b.Title.ToLower().Contains(title.ToLower())).ToList();
                return Ok(books);
            }

            if (title is null && author is not null)
            {
                books = _bookCollection.Where(b =>
                    b.Author.Equals(author, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.ToLower().Contains(author.ToLower())).ToList();
                return Ok(books);

            }

            books = _bookCollection.Where(b =>
                (b.Title.Equals(title, StringComparison.OrdinalIgnoreCase) ||
                 b.Title.ToLower().Contains(title!.ToLower())) &&
                (b.Author.Equals(author, StringComparison.OrdinalIgnoreCase) ||
                 b.Author.ToLower().Contains(author!.ToLower()))
            ).ToList();
            return Ok(books);

        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            _logger.LogError("Book not found");
            return NotFound("Book not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
/// <summary>
/// Gives a list of availble books
/// </summary>
/// <response code="200">Returns a list of available books</response>
/// <response code="400">Something went wrong</response>
    [HttpGet("available")]
    public IActionResult GetByAvailable()
    {
        try
        {
            var books = _bookCollection.Where(b => b.Status == Books.Availability.Available).OrderBy(b => b.Title)
                .ToList();
            return Ok(books);
        }
        catch (Exception)
        {
            _logger.LogError("Books not found");
            return NotFound("Books not found");
        }
    }

/// <summary>
/// Gives a list of unavailable books
/// </summary>
/// <response code="200">Returns a list of unavailable books</response>
/// <response code="400">Something went wrong</response>
    [HttpGet("unavailable")]
    public IActionResult GetByUnavailable()
    {
        try
        {
            var books = _bookCollection.Where(b => b.Status == Books.Availability.Unavailable).OrderBy(b => b.Title)
                .ToList();
            return Ok(books);
        }
        catch (Exception)
        {
            _logger.LogError("Books not found");
            return NotFound("Books not found");
        }
    }
   
    
    /// <summary>
    /// Add new Book
    /// </summary>
    /// <param name="newBook">
    ///NewBook will be converted in a Struct, it needs to have:<br/>
    /// Title<br/>
    /// Author<br/>
    /// ISBN<br/>
    /// </param>
    /// <response code="201">Book successfully added</response>
    /// <response code="400">Something went wrong</response>
    [HttpPost]
    public IActionResult Add(NewBook newBook)
    {
        try
        {
            var newBookData = new Books()
            {
                Title = newBook.Title,
                Author = newBook.Author,
                ISBN = newBook.ISBN,
            };
            //No Checks Needed
            
            var newAdd = _bookCollection.Add(newBookData);
            return Created("RedirectLink",newAdd.Id);
        }
        //If there were checks I would have catched exceptions here
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Change one or more properties on the underlying object
    /// </summary>
    /// <param name="updateBook">
    /// This item must contain the Id and
    /// the string/strings that you want to modify.<br/>
    /// If you only want to change one field just send the Id<br/>
    /// and the string you want to change
    /// </param>
    /// <param name="id"></param>
    /// <response code="204">Update was successfull</response>
    /// <response code="400">Something went wrong</response>
    /// <response code="404">Id doesn't exist</response>
    [HttpPut("{id}")]
    public IActionResult Update(UpdateBook updateBook)
    {
        var toUpdate = new Books
        {
            Id = updateBook.Id,
            Title = "",
            Author = "",
            ISBN = "",
        };
        Type userType = updateBook.GetType();
        PropertyInfo[] properties = userType.GetProperties();
        try
        {
            _bookCollection.GetById(updateBook.Id);
            foreach (var property in properties)
            {
                if (property.GetValue(updateBook) != null)
                {
                    if (property.Name == "ISBN")
                    {
                        HandleIsbnValidation(new Books()
                        {
                            Author = "",
                            Title = "",
                            ISBN = updateBook.ISBN!,
                        });
                    }
                    var toUpdateProperty = typeof(Books).GetProperty(property.Name);
                    toUpdateProperty!.SetValue(toUpdate, property.GetValue(updateBook));
                }
            }
            _bookCollection.Update(toUpdate);
            return NoContent();
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            _logger.LogError("Id not found");
            return NotFound("Id not found");
        }
        catch (InvalidIsbnException)
        {
            _logger.LogError("ISBN invalid");
            return BadRequest("ISBN invalid");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// This put request takes care alone of the available / unavailable switch
    /// </summary>
    /// <param name="id">book that is being reserved or given back </param>
    /// <response code="204">Status switched to opposite successfully</response>
    /// <response code="400">Something went wrong</response>
    /// <response code="404">Id doesn't exist</response>
    [HttpPut("toggleStatus/{id}")]
    public IActionResult ToggleStatus(int id)
    {
        try
        {
            _bookCollection.GetById(id);
            _bookCollection.ToggleAvailability(id);
            return NoContent();
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            _logger.LogError("Id not found");
            return NotFound("Id not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete one element from the Database
    /// </summary>
    /// <param name="id">
    ///Id is just the int of the element that you want to delete.
    /// </param>
    /// <response code="204">Id deleted successfully</response>
    /// <response code="400">Something went wrong</response>
    /// <response code="404">Id not found</response>
    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute]int id)
    {
        try
        {
            _bookCollection.GetById(id);
            _bookCollection.Delete(id);
            return NoContent();
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            _logger.LogError("Id not found");
            return NotFound("Id not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    private void HandleIsbnValidation(Books book)
    {
        new Manager(new IsbnCheck(book.ISBN), book).Check();
    }
    
}