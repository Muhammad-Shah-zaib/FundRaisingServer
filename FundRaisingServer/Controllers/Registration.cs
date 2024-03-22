using System.Runtime.CompilerServices;
using System.Text;
using FundRaisingServer.Models;
using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Services.PasswordHashing;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Registration(FundRaisingDbContext context, IArgon2Hasher argon2Hasher): ControllerBase
{
    private readonly FundRaisingDbContext _context = context;
    private readonly IArgon2Hasher _argon2Hasher = argon2Hasher;
    
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
        byte[] password = Encoding.UTF8.GetBytes(registrationRequestDto.Password);
        byte[] salt = Encoding.UTF8.GetBytes(RandomSaltGenerator.GenerateSalt(512 / 8));
        var hashedPassword = this._argon2Hasher.HashPassword(password: password, salt: salt);
        Console.WriteLine("PASSWORD HASHED: " + hashedPassword);

        registrationRequestDto.Password = hashedPassword;
        var result = await this._context.Users.AddAsync(new User()
        {
            FirstName = registrationRequestDto.FirstName,
            LastName = registrationRequestDto.LastName,
            Email=registrationRequestDto.Email,
        });

        await this._context.SaveChangesAsync();
        return Ok("Returning the id" + result.Entity.UserId);
    }

    private async Task<User> GetUserByEmail( string email )
    {
        // getting the user if exists
        var query = $"Select * FROM Users Where Email = '{email}'";
        var user = await this._context.Users.FromSqlRaw(query).FirstOrDefaultAsync();
        return user;
    }
}