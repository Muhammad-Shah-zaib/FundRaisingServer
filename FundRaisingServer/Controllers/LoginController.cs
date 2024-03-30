using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController(LoginService loginService): ControllerBase
{
    //DIs
    private readonly LoginService _loginService = loginService;

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest("Please provide both Email and Password");
            // checking for valid credentials
            var result = await this._loginService.LoginAsync(request);
            if (result == null) return BadRequest();
        
            return Ok(result!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
