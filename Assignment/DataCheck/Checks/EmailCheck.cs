using System.Text.RegularExpressions;
using Assignment.Data.Collections;
using Assignment.Data.Interfaces;

namespace Assignment.DataCheck.Checks;

public class EmailCheck
{
    private readonly CustomerCollection _collection;


    public void Check(string email)
    {
        var emailRegex = new Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
        if(!emailRegex.IsMatch(email)){
            //TODO:Throw different exception email not valid
            throw new Exception(); 
        }
        var user = _collection.FirstOrDefault(customer=> customer.Email == email);
        if (user != null)
        {
            //TODO:Exceptio for mail already used
            throw new Exception();
        }
        
    }


}