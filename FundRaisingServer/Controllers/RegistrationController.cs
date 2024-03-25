using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController(FundRaisingDbContext context, IUserRepository userRepo, IPasswordRepository passwordRepo, IUserAuthLogRepository userAuthLogRepo): ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IPasswordRepository _passwordRepo = passwordRepo;
    private readonly IUserAuthLogRepository _userAuthLogRepo = userAuthLogRepo;
    
    [HttpPost]
    public async Task<ActionResult<RegistrationResponseDto>> Register([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        try
        {
            /*
             * Since the DataAnnotations are used so no need for
             * manually checking for the *particular modelState* , Since it
             * is already done by Framework
             */
            if (!ModelState.IsValid) return BadRequest(ModelState);
            

            // checking if the Email already exists or not
            if ( (await this._userRepo.GetUserByEmailAsync(registrationRequestDto.Email)) != null )
                return StatusCode(409, "Conflict: Email Already Exists");
        
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


            // Saving Logs
            if (!(await this._userAuthLogRepo.SaveUserAuthLogAsync(registrationRequestDto.Email,
                    UserEventType.Registration)))
            {
                await this._userRepo.DeleteUserByEmailAsync(registrationRequestDto.Email);
                await this._passwordRepo.DeleteUserPasswordByEmailAsync(registrationRequestDto.Email);
                return StatusCode(500, "Internal Server error please try again later");
            };
            
            // Returning the response Dto
            return Ok(new RegistrationResponseDto()
            {
                Success = true,
                Email = registrationRequestDto.Email, // this will be replaced by JWT TOKEN
                Message = "User has been Registered Successfully",
                Errors = null
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Something went Wrong");
        }

        
    }
    
}