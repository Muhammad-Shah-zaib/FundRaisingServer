using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Registration(FundRaisingDbContext context, IUserRepository userRepo, IPasswordRepository passwordRepo, IUserAuthLogRepository userAuthLogRepo): ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IPasswordRepository _passwordRepo = passwordRepo;
    private readonly IUserAuthLogRepository _userAuthLogRepo = userAuthLogRepo;
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        try
        {
            // checking if the model state is valid or not
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
            if ( (await this._userRepo.GetUserByEmailAsync(registrationRequestDto.Email)) != null )
                return BadRequest("Email Already Exists");
        
            // saving User
            if (!(await this._userRepo.SaveUserAsync(registrationRequestDto)))
            {
                return StatusCode(500, "Failed to save User");
            }

           
            // saving password of the user
            if (!(await this._passwordRepo.SaveUserPasswordAsync(registrationRequestDto.Email,
                    registrationRequestDto.Password)))
            {
                await this._userRepo.DeleteUserByEmailAsync(registrationRequestDto.Email);
                return StatusCode(500, "Failed to save User Password");
            }


            if (!(await this._userAuthLogRepo.SaveUserAuthLogAsync(registrationRequestDto.Email,
                    UserEventType.Registration)))
            {
                await this._userRepo.DeleteUserByEmailAsync(registrationRequestDto.Email);
                return StatusCode(500, "Failed to save the User Log");
            };
            return Ok("User Registered Successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Something went Wrong");
        }
    }
    
}