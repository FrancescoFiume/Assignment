using Assignment.Data;
using Assignment.Data.Interfaces;
using Assignment.Data.Models;
using Assignment.DataCheck.Checks.CustomerChecks;
using Assignment.DTO;

namespace Assignment.DataCheck;

public class Manager
{
    private readonly EmailCheck _emailCheck;
    private IObjectDb _toCheck;



    public Manager(EmailCheck emailCheck, IObjectDb toCheck)
    {
        _emailCheck = emailCheck;
        _toCheck = toCheck;
    }

    public void Check()
    {
        switch (_toCheck)
        {
            case (Customers customer):
            {
                
                _emailCheck.Check(customer.Email);
                break;
            }
            case (Books book):
            {
                break;
            }
        }
        
        
       

    }
    
    
}