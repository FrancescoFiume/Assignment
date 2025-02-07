using Assignment.CustomExceptions;
using Assignment.Data.Collections;
using Assignment.Data.Models;
using Assignment.DataCheck;
using Assignment.DataCheck.Checks.ReservationCheck;
using Assignment.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers;
/// <summary>
/// Contains CRUD operations for Reservation
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationCollection _reservationCollection;
    private readonly ILogger<CustomerController> _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="reservationCollection">Dependency Injected books Collection</param>
    /// <param name="logger">Dependency Injected logger</param>
    public ReservationController(ReservationCollection reservationCollection,
        ILogger<CustomerController> logger)
    {
        _logger = logger;
        _reservationCollection = reservationCollection;

    }
    /// <summary>
    /// Fetch all the reservations
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
    public IActionResult GetCustomerReservations([FromRoute] int customerId)
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
    /// <summary>
    /// Create a new Reservation
    /// </summary>
    /// <param name="newReservation">this object needs to have only two valid fields, Book Id and Customer Id</param>
    /// <response code="201">Reservation created, you will get back the whole object with also the Book and Customer details</response>
    /// <response code="400">One of the following happened: BookId Incorrect, CustomerId Incorrect, or something else happened, read body</response>
    [HttpPost]
    public IActionResult Add(NewReservation newReservation)
    {
        var reservation = new Reservations()
        {
            BookId = newReservation.BookId,
            CustomerId = newReservation.CustomerId,
        };
        try
        {
            new Manager(new BookReservedChecks(_reservationCollection, newReservation.BookId), reservation).Check();
            reservation = _reservationCollection.Add(reservation);
            return Created("redirect Url", reservation);

        }
        catch (BookReservedException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Book reservation already exists");
        }
        //I'm delegating to the DB to check if the id of customer and book are correct, if they are not, then it will throw either one of those
        catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
        {
            _logger.LogError(e.Message);
            return BadRequest("BookId or Customer Id is not valid");
        }
    }

    /// <summary>
    /// Extend expiration Date
    /// </summary>
    /// <param name="reservationId">Id of the reservation that you want to extend</param>
    /// <param name="newDate">new date(must be YYYY-MM-DD)</param>
    /// <response code="204">Reservation created updated successfully</response>
    /// <response code="400">Something went wrong, read body</response>
    /// <response code="404">Reservation Id doesn't exist</response>


    [HttpPut("{reservationId}")]
    public IActionResult ExtendReservation([FromRoute] int reservationId, DateTime newDate)
    {
        try
        {
            _reservationCollection.GetById(reservationId);
            _reservationCollection.SetCustomExpiration(reservationId, newDate);
            return NoContent();
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            _logger.LogError(e.Message);
            return NotFound("Reservation Id doesn't exist");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete Reservation
    /// </summary>
    /// <param name="reservationId">Id of the reservation that you want to delete</param>
    /// <response code="204">Item deleted successfully</response>
    /// <response code="400">Something went wrong, read body</response>
    /// <response code="404">Reservation Id doesn't exist</response>
    [HttpDelete("{reservationId}")]
    public IActionResult RemoveReservation([FromRoute] int reservationId)
    {
        try
        {
            _reservationCollection.GetById(reservationId);
            _reservationCollection.Delete(reservationId);
            return NoContent();
        }
        catch (Exception e) when (e is ArgumentNullException || e is InvalidOperationException)
        {
            _logger.LogError(e.Message);
            return NotFound("Reservation Id doesn't exist");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }


}