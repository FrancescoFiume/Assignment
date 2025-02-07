using Assignment.CustomExceptions;
using Assignment.Data.Collections;

namespace Assignment.DataCheck.Checks.ReservationCheck;

/// <summary>
/// This class checks if the book is already reserved.
/// </summary>
public class BookReservedChecks : ICheck
{
    private readonly ReservationCollection _reservationCollection;
    private int _bookId;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="reservationCollection">Collection of Reservations</param>
    /// <param name="bookId">Id of the book that you want to check</param>
    public BookReservedChecks(ReservationCollection reservationCollection, int bookId)
    {
        _reservationCollection = reservationCollection;
        this._bookId = bookId;
    }
    /// <summary>
    /// Check Method runs a query and if it finds a book in the reservations that has the same Id as the requested Id, then it throws an error
    /// </summary>
    /// <exception cref="BookReservedException">Custom execption that gets managed in the Controller</exception>
    public void Check()
    {
        if (_reservationCollection.FirstOrDefault(reservation => reservation.BookId == _bookId) != null)
        {
            throw new BookReservedException();
        }


    }
}