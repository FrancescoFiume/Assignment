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
    /// <returns>returns the whole collection as is</returns>
    /// 
    [HttpGet]
    public IActionResult All()
    {
        return Ok(_bookCollection);
    }
    
    /// <summary>
    /// Fetch specific customer by Id, if the user doesn't exist returns NotFound
    /// </summary>
    /// <param name="id">Id must be an integer</param>
    /// <returns>returns the user that has the requested id</returns>
    [HttpGet("{id}")]
    public  IActionResult GetOne(int id)
    {
        try
        {
            var user = _bookCollection.GetById(id);
            return Ok(user);
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            
            _logger.LogError("Id not found");
            return NotFound("Id not found");
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
    /// <returns>
    /// Returns 200 and the newly created user Id if everything goes straight,<br/>
    /// BadRequest if anything goes wrong
    /// </returns>
    [HttpPost("new")]
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
            return Ok(newAdd.Id);
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
    /// <returns>
    /// Not Found if the Id does not exist in the database,<br/>
    /// Bad Request if anything goes wrong <br/>
    /// No Content (code 204) if everything goes smoothly
    /// </returns>
    [HttpPut("update")]
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
    /// <returns></returns>
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
    /// <returns>
    ///Returns:<br/>
    ///Not Found if the Id doesn't exist,<br/>
    /// No Content (status 204) if the id exist and the deletion goes ok
    /// </returns>
    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
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