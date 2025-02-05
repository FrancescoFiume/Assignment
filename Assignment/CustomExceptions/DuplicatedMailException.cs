namespace Assignment.CustomExceptions;

public class DuplicatedMailException:Exception
{
    public DuplicatedMailException() : base("Duplicated mail")
    {
        
    }
}