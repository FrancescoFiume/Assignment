using Assignment.Data.Collections;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;
[ApiController]
 [Route("api/v1/[controller]")]

public class CustomerController :ControllerBase
{
    private readonly CustomerCollection _customerCollection;

    public CustomerController(CustomerCollection customerCollection)
    {
        _customerCollection = customerCollection;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(_customerCollection);
    }
    

    
}