using System.ComponentModel.DataAnnotations;
using Assignment.CustomExceptions;
using Assignment.DataCheck;
using Assignment.DataCheck.Checks.BookChecks;
using Assignment.Db.Models;

namespace Assignment.Attributes;

public sealed class IsbnAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string isbn)
            return ValidationResult.Success;
        
        try
        {
            new Manager(new IsbnCheck(isbn), new BookDbo
            {
                ISBN = isbn,
                Title = "",
                Author = ""
            }).Check();
            
            return ValidationResult.Success;
        }
        catch (InvalidIsbnException e)
        {
            return new ValidationResult("Invalid ISBN");
        }
    }
}