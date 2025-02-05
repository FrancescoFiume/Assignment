using System.Text.RegularExpressions;
using Assignment.CustomExceptions;
using Assignment.Data.Collections;
using Assignment.Data.Interfaces;

namespace Assignment.DataCheck.Checks;

public class EmailCheck
{
    private readonly CustomerCollection _collection;


    public EmailCheck(CustomerCollection collection)
    {
        _collection = collection;
    }
    public void Check(string email)
    {
        var emailRegex = new Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
        if(!emailRegex.IsMatch(email))
        {

            throw new InvalidEmailFormatExceptions();
        }
        var user = _collection.FirstOrDefault(customer=> customer.Email == email);
        if (user != null)
        {
            throw new DuplicatedMailException();
        }
        
    }


}