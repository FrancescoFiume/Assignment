using Assignment.Data;
using Assignment.Data.Models;
using Assignment.DataCheck.Checks.BookChecks;
using Assignment.DataCheck.Checks.CustomerChecks;

namespace Assignment.DataCheck;
/// <summary>
/// Manager takes care of the all the checks on all the data (either from add or update
/// that are about to go in the db
/// </summary>
public class Manager
{
    private readonly EmailCheck _emailCheck;
    private IObjectDb _toCheck;
    private ISBNCheck _isbnCheck;


/// <summary>
/// Constructor
/// </summary>
/// <param name="emailCheck">Email Checker</param>
/// <param name="toCheck">Class that implements IObjectDb that needs the checkin'</param>
    public Manager(EmailCheck emailCheck, IObjectDb toCheck)
    {
        _emailCheck = emailCheck;
        _toCheck = toCheck;
    }

    public Manager(ISBNCheck isbnCheck, IObjectDb toCheck)
    {
        _toCheck = toCheck;
        _isbnCheck = isbnCheck;
    }


/// <summary>
/// This function is pretty straightforward, it's just a switch case which tries to cast the<br/>
/// argument into one of the classes that implements the underlying interface.<br/>
/// if the cast is successfull then it perform the checks of that class.
/// </summary>
    public void Check()
    {
        switch (_toCheck)
        {
            case (Customers customer):
            {
                
                _emailCheck.Check(customer.Email);
                //EmailCheck is the only one that made sense to me.
                return;
            }
            case (Books book):
            {
                _isbnCheck.Check();
                return;
            }
        }
        
        
       

    }
    
    
}