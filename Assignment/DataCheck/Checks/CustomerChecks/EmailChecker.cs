using System.ComponentModel.DataAnnotations;
using Assignment.CustomExceptions;
using Assignment.Db;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Assignment.DataCheck.Checks.CustomerChecks;

public sealed class EmailChecker
{
    private readonly AppDbContext _db;

    public EmailChecker(AppDbContext db)
    {
        _db = db;
    }

    public async Task CheckMailAsync(
        string email,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken
    )
    {
        var isValidEmail = new EmailAddressAttribute().IsValid(email);
        if (!isValidEmail)
        {
            modelState.AddModelError("Email", "Invalid email");
            return;
        }

        var isDuplicate = await _db.Customers
            .Where(x => x.Email.ToUpper() == email.ToUpper())
            .AnyAsync(cancellationToken);
        
        if (isDuplicate)
            modelState.AddModelError("Email", "Email already exists");
    }
}

public abstract class EmailCheckResult
{
    public sealed class Success : EmailCheckResult;
    public sealed class InvalidEmail : EmailCheckResult;
    public sealed class DuplicatedEmail : EmailCheckResult;
}