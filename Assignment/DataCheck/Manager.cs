using Assignment.Data.Interfaces;
using Assignment.DataCheck.Checks;

namespace Assignment.DataCheck;

public class Manager
{
    private readonly EmailCheck _emailCheck;



    public Manager(EmailCheck emailCheck)
    {
        _emailCheck = emailCheck;
    }

    public void Check()
    {
        
    }
    
    
}