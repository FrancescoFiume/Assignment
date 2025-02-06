using System.Text.RegularExpressions;
using Assignment.CustomExceptions;

namespace Assignment.DataCheck.Checks.BookChecks;

public class ISBNCheck
{
    private string ISBN { get; }

    public ISBNCheck(string isbn)
    {
        ISBN = isbn;
    }

    public void Check()
    { 
        Regex isbn10Regex = new Regex(@"^\d{9}[\dX]$");
        Regex isbn13Regex = new Regex(@"^\d{13}$");
        if (ISBN.Length == 10 && isbn10Regex.IsMatch(ISBN))
        {
            if (IsValidISBN10())
            {
                return;
            }
        }
        else if (ISBN.Length == 13 && isbn13Regex.IsMatch(ISBN))
        {
            if (IsValidISBN13())
            {
                return;
            }
        }

        throw new InvalidISBNException();

    }
    
    private  bool IsValidISBN10()
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

    private bool IsValidISBN13()
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