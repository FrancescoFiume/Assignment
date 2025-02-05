using Assignment.Data.Interfaces;
using Assignment.DataCheck.Checks;
using Assignment.DTO;

namespace Assignment.DataCheck;

public class Manager
{
    private readonly EmailCheck _emailCheck;
    private NewCustomer _customer;



    public Manager(EmailCheck emailCheck, NewCustomer customer)
    {
        _emailCheck = emailCheck;
        _customer = customer;
    }

    public void Check()
    {
        _emailCheck.Check(_customer.Email);
        
       

    }
    
    
}