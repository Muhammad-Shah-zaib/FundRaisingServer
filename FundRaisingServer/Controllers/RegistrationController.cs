using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("Registration")]
public class RegistrationController(FundRaisingDbContext context,IUserTypeRepository userTypeService, IUserRepository userRepo, IPasswordRepository passwordRepo, IUserAuthLogRepository userAuthLogRepo): ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IPasswordRepository _passwordRepo = passwordRepo;
    private readonly IUserAuthLogRepository _userAuthLogRepo = userAuthLogRepo;
    private readonly IUserTypeRepository _userTypeService = userTypeService;
    
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
                return StatusCode(409, "Email Already Exists");
        
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
                await this._passwordRepo.DeleteUserPasswordByEmailAsync(registrationRequestDto.Email);
                await this._userRepo.DeleteUserByEmailAsync(registrationRequestDto.Email);
                return StatusCode(500, "Internal Server error please try again later");
            };

            // Saving user types
            if (!await this._userTypeService.SaveUserTypeAsync(new UserTypeDto()
                {
                    UserType = registrationRequestDto.UserType,
                    Email = registrationRequestDto.Email
                }))
            {
                await this._passwordRepo.DeleteUserPasswordByEmailAsync(registrationRequestDto.Email);
                await this._userRepo.DeleteUserByEmailAsync(registrationRequestDto.Email);
                // need to implement a method for deleting the logs
                return StatusCode(500, "Failed so save user");
            }
            
            // Returning the response Dto
            return Ok(new RegistrationResponseDto()
            {
                Success = true,
                Email = registrationRequestDto.Email,
                Message = "User has been Registered Successfully",
                Errors = null
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        
    }


    [HttpGet]
    [Route("Test")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> testing()
    {
        return Ok("I am working");
    }
    
}