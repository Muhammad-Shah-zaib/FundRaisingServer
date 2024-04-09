using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController(ILoginRepository loginService): ControllerBase
{
    //DIs
    private readonly ILoginRepository _loginService = loginService;

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest("Please provide both Email and Password");
            // checking for valid credentials
            var result = await this._loginService.LoginAsync(request);
            if (result == null) return Unauthorized("Invalid Email or Password");
        
            return Ok(result!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
