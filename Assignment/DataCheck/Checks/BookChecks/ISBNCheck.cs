using System.Text.RegularExpressions;
using Assignment.CustomExceptions;

namespace Assignment.DataCheck.Checks.BookChecks;

/// <summary>
/// This class ensures that the isbn is correct
/// </summary>
public class IsbnCheck : ICheck
{
    /// <summary>
    /// Isbn string
    /// </summary>
    private string ISBN { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="isbn">isbn string</param>
    public IsbnCheck(string isbn)
    {
        ISBN = isbn;
    }

    /// <summary>
    /// this function has 4 parts.<br/>
    /// First: creates the 2 regex (there are 2 types of valid isbn)
    /// Second: Checks if isbn is valid for 10 digits, if so returns, no error
    /// Thrid: Checks if the isbn is falid for 13 digits
    /// Fouth: Throws error
    /// </summary>
    /// <exception cref="InvalidIsbnException">if the isbn is invalid throws a custom error</exception>
    public void Check()
    {
        Regex isbn10Regex = new Regex(@"^\d{9}[\dX]$");
        Regex isbn13Regex = new Regex(@"^\d{13}$");
        if (ISBN.Length == 10 && isbn10Regex.IsMatch(ISBN))
        {
            if (IsValidIsbn10())
            {
                return;
            }
        }
        else if (ISBN.Length == 13 && isbn13Regex.IsMatch(ISBN))
        {
            if (IsValidIsbn13())
            {
                return;
            }
        }

        throw new InvalidIsbnException();

    }
    /// <summary>
    /// Checks for the 10 digits
    /// </summary>
    /// <returns>
    ///returns true if the isbn is valid
    /// </returns>
    private bool IsValidIsbn10()
    {
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(ISBN[i])) return false;
            sum += (ISBN[i] - '0') * (10 - i);
        }
        char checksum = ISBN[9];
        sum += (checksum == 'X') ? 10 : (checksum - '0');
        return sum % 11 == 0;
    }

    /// <summary>
    /// Checks for the 13 digits
    /// </summary>
    /// <returns>
    ///returns true if the isbn is valid
    /// </returns>
    private bool IsValidIsbn13()
    {
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            if (!char.IsDigit(ISBN[i])) return false;
            sum += (ISBN[i] - '0') * (i % 2 == 0 ? 1 : 3);
        }
        int checksum = (10 - (sum % 10)) % 10;
        return checksum == (ISBN[12] - '0');
    }

}