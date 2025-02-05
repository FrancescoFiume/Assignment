using Assignment.CustomExceptions;
using Assignment.Data.Collections;
using Assignment.DataCheck;
using Assignment.DataCheck.Checks;
using Assignment.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;
[ApiController]
 [Route("api/v1/[controller]")]

public class CustomerController :ControllerBase
{
    private readonly CustomerCollection _customerCollection;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(CustomerCollection customerCollection, ILogger<CustomerController> logger)
    {
        _customerCollection = customerCollection;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult All()
    {
        return Ok(_customerCollection);
    }

    [HttpGet("{id}")]
    public  IActionResult GetOne(int id)
    {
        var user = _customerCollection.FirstOrDefault(c => c.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Ciao sto documentando
    /// </summary>
    /// <param name="newCustomer"></param>
    /// <returns></returns>
    [HttpPost("/new")]
    public IActionResult Add(NewCustomer newCustomer)
    {
        try
        {
            new Manager(new EmailCheck(_customerCollection), newCustomer).Check();
        }
        catch (InvalidEmailFormatExceptions)
        {
            _logger.LogError("Invalid email format");
            return BadRequest();
        }
        catch (DuplicatedMailException)
        {
            _logger.LogError("Duplicated mail");
            return BadRequest();
        }
        
        return Ok();
    }
    
    

    
}