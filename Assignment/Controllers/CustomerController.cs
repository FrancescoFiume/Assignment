using System.Reflection;
using Assignment.CustomExceptions;
using Assignment.Data.Collections;
using Assignment.Data.Models;
using Assignment.DataCheck;
using Assignment.DataCheck.Checks.CustomerChecks;
using Assignment.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;

/// <summary>
/// Contains CRUD operations for customer
/// </summary>
[ApiController]
 [Route("api/v1/[controller]")]
public class CustomerController :ControllerBase
{
    private readonly CustomerCollection _customerCollection;
    private readonly ILogger<CustomerController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="customerCollection">Dependency Injected customer Collection</param>
    /// <param name="logger">Dependency Injected logger</param>
    public CustomerController(CustomerCollection customerCollection, ILogger<CustomerController> logger)
    {
        _customerCollection = customerCollection;
        _logger = logger;
    }
    
    /// <summary>
    /// Fetch all the Customers
    /// </summary>
    /// <returns>returns the whole collection as is</returns>
    /// 
    [HttpGet]
    public IActionResult All()
    {
        return Ok(_customerCollection);
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
            var user = _customerCollection.GetById(id);
            return Ok(user);
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            
            _logger.LogError("Id not found");
            return NotFound("Id not found");
        }
    }
    
    /// <summary>
    /// Add new Customer
    /// </summary>
    /// <param name="newCustomer">
    ///NewCustomer will be converted in a Struct, it needs to have:<br/>
    /// First Name<br/>
    /// Last Name<br/>
    /// Mail<br/>
    /// NB: The mail will be checked for both pattern and duplication in the codebase.
    /// </param>
    /// <returns>
    /// Returns 200 and the newly created user Id if everything goes straight,<br/>
    /// BadRequest if the mail is invalid or present already in the codebase
    /// </returns>
    [HttpPost("new")]
    public IActionResult Add(NewCustomer newCustomer)
    {
        try
        {
            var newCustomerData = new Customers()
            {
                FirstName = newCustomer.FirstName,
                LastName = newCustomer.LastName,
                Email = newCustomer.Email,
            };
            HandleEmailValidation(newCustomerData);
            var newAcc = _customerCollection.Add(newCustomerData);
            return Ok(newAcc.Id);
            //If this were an actual registration this should have returned
            //1) Id
            //2)Redirect page (The redirected page is a static page asking for mail to be verified.
        }
        catch (InvalidEmailFormatExceptions)
        {
            _logger.LogError("Invalid email format");
            return BadRequest("Invalid email format");
        }
        catch (DuplicatedMailException)
        {
            _logger.LogError("Duplicated mail");
            return BadRequest("Duplicated mail");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Change one or more properties on the underlying object
    /// </summary>
    /// <param name="newCustomer">
    /// This item must contain the Id and
    /// the string/strings that you want to modify.<br/>
    /// If you only want to change one field just send the Id<br/>
    /// and the string you want to change
    /// </param>
    /// <returns>
    /// Not Found if the Id does not exist in the database,<br/>
    /// Not Found if the email that you want to set is either incorrect or duplicated<br/>
    /// No Content (code 204) if everything goes smoothly
    /// </returns>
    [HttpPut("update")]
    public IActionResult Update(UpdateCustomer newCustomer)
    {
        Customers toUpdate = new Customers
        {
            Id = newCustomer.Id,
            FirstName = "",
            LastName = "",
            Email = "",
        };
        Type userType = newCustomer.GetType();
        PropertyInfo[] properties = userType.GetProperties();
        try
        {
            _customerCollection.GetById(newCustomer.Id);
            foreach (var property in properties)
            {
                if (property.GetValue(newCustomer) != null)
                {
                    if (property.Name == "Email")
                    {
                        try
                        {
                            HandleEmailValidation(new Customers()
                            {
                                FirstName = "",
                                LastName = "",
                                Email = newCustomer.Email!,
                            });
                        }
                        catch (InvalidEmailFormatExceptions)
                        {
                            _logger.LogError("Invalid email format");
                            return BadRequest("Invalid email format");
                        }
                        catch (DuplicatedMailException)
                        {
                            _logger.LogError("Duplicated mail");
                            return BadRequest("Duplicated mail");
                        }
                    }
                    var toUpdateProperty = typeof(Customers).GetProperty(property.Name);
                    toUpdateProperty!.SetValue(toUpdate, property.GetValue(newCustomer));

                }
            }
            _customerCollection.Update(toUpdate);
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
            _customerCollection.GetById(id);
            _customerCollection.Delete(id);
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


    private void HandleEmailValidation(Customers customer)
    {
            new Manager(new EmailCheck(_customerCollection), customer).Check();
    }
}