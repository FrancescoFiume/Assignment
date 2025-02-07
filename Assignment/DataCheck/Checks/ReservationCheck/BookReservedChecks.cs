using Assignment.CustomExceptions;
using Assignment.Data.Collections;

namespace Assignment.DataCheck.Checks.ReservationCheck;

public class BookReservedChecks:ICheck
{
    private readonly ReservationCollection _reservationCollection;
    private int _bookId;

    public BookReservedChecks(ReservationCollection reservationCollection, int bookId)
    {
        _reservationCollection = reservationCollection;
        this._bookId = bookId;
    }
    public void Check()
    {
        if (_reservationCollection.FirstOrDefault(reservation => reservation.BookId == _bookId) != null)
        {
            throw new BookReservedException();
        }
       

    }
}