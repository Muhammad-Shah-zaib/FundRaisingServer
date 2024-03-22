using System.Runtime.CompilerServices;
using FundRaisingServer.Models;
using FundRaisingServer.Models.DTOs.UserAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Registration(FundRaisingDbContext context): ControllerBase
{
    private FundRaisingDbContext _context = context;
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        // checking if the mdoelstate is valid or not
        if (!ModelState.IsValid)
        {
            if (registrationRequestDto.FirstName == string.Empty)
                return BadRequest("First Name is not provided");
            if (registrationRequestDto.Email == string.Empty)
                return BadRequest("Email is not provided");
            if (registrationRequestDto.Password == string.Empty)
                return BadRequest("Password is not provided");
        }

        // checking if the Email already exists or not
        var user = this.GetUserByEmail(registrationRequestDto.Email).Result;

        if (user != null)
        {
            return BadRequest("Email Already Exists");
        }
        // Hashing the password
        return Ok("Registered");
    }

    private async Task<User> GetUserByEmail( string email )
    {
        // getting the user if exists
        var query = $"Select * FROM Users Where Email = '{email}'";
        var user = await this._context.Users.FromSqlRaw(query).FirstOrDefaultAsync();
        return user;
    }
}