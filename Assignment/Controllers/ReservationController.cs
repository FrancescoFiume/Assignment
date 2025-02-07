using Assignment.Data.Collections;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;
/// <summary>
/// Contains CRUD operations for Reservation
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ReservationController:ControllerBase
{
    private readonly ReservationCollection _reservationCollection;
    private readonly ILogger<CustomerController> _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="reservationCollection">Dependency Injected books Collection</param>
    /// <param name="logger">Dependency Injected logger</param>
    public ReservationController(ReservationCollection reservationCollection, ILogger<CustomerController> logger)
    {
        _logger = logger;
        _reservationCollection = reservationCollection;
        
    }
    /// <summary>
    /// Fetch all the Books
    /// </summary>
    /// <response code="200">Returns list of reservation, each one has book and user data</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet]
    public IActionResult All()
    {
        try
        {
            return Ok(_reservationCollection);

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Fetch specific reservation by customer Id, if the customer doesn't exist returns not found
    /// </summary>
    /// <param name="customerId">Id must be an integer</param>
    /// <returns>returns the reservation that has the requested id</returns>
    /// <response code="200">The object returned is a list of reservations, each one has its book data and customer data attache </response>
    /// <response code="404">Customer does not have any reservations</response>
    /// <response code="400">Something went wrong</response>
    [HttpGet("{customerId}")]
    public  IActionResult GetCustomerReservations([FromRoute]int customerId)
    {
        try
        {
            var reservations = _reservationCollection.Where(customer => customer.Customer.Id == customerId).ToList();

            return Ok(reservations);
        }
        catch (Exception e) when (e is ArgumentNullException)
        {

            _logger.LogError("Customer has no reservations");
            return NotFound("Customer has no reservations");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    

}