using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class ValidateTokenController: ControllerBase
{
    [HttpGet]
    // This will check and validate the JWT token presence
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Testing()
    {
        return Ok("Token is Valid");
    }    
}