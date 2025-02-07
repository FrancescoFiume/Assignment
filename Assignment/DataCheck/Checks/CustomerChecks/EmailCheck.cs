using System.Text.RegularExpressions;
using Assignment.CustomExceptions;
using Assignment.Data.Collections;

namespace Assignment.DataCheck.Checks.CustomerChecks;

/// <summary>
/// This class is part of the Check ecosystem. Relates to the string Mail from Customer
/// </summary>
public class EmailCheck:ICheck
{
    private readonly CustomerCollection _collection;
    private string _email;

    #pragma warning disable CS1591
    public EmailCheck(CustomerCollection collection, string email)
    {
        _collection = collection;
        _email = email;
    }
    /// <summary>
    /// There is 2 parts in this function<br/>
    /// First: Check if the mail is pseudovalid (it's just a Regex pattern that looks for string@string.subdomain<br />
    /// Second: Check if the mail is in the db already<br/>
    /// if any of those checks are failed it throws a custom error that gets catched in the controller for a more filtered response
    /// </summary>
    /// <param name="email">string that needs to be checked</param>
    /// <exception cref="InvalidEmailFormatExceptions">Custom exception if the regex fails</exception>
    /// <exception cref="DuplicatedMailException">Custom exeption if the mail is in the db already</exception>
    public void Check()
    {
        var emailRegex = new Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
        if(!emailRegex.IsMatch(_email))
        {

            throw new InvalidEmailFormatExceptions();
        }
        var user = _collection.FirstOrDefault(customer=> customer.Email == _email);
        if (user != null)
        {
            throw new DuplicatedMailException();
        }
        
    }


}