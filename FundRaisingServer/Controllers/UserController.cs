using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class UserController (IUserRepository userRepo, IUserAuthLogRepository userAuthLogRepo): ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IUserAuthLogRepository _userAuthLogRepo = userAuthLogRepo;
    
    /*
     * The method below is going to
     * return an enumerable object of
     * users containing all the information
     * related to user, including his/her logs
     */
    [HttpGet]
    [Route("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
    {
        return Ok(await this._userRepo.GetAllUsersAsync());
    }

    [HttpPut]
    [Route("UpdateUser")]
    
    /*
     * below api method is going to
     * update the user from the data
     * of type UpdateUserRequestDto
     * and then update the log of Update
     * and return the single updated User
     * with updated logs
     */
    public async Task<ActionResult<UserResponseDto>> UpdateUser(UserUpdateRequestDto userUpdateRequestDto)
    {
        /*
         * we need to check the model state
         * since there are already DataAnnotations
         * for the UpdateUserRequestDto so no need to
         * manually check the Model State
         */
        if (!ModelState.IsValid) return BadRequest("Please provide all the user details");
        
        // now we need to update the user
        try
        {
            await this._userRepo.UpdateUserAsync(userUpdateRequestDto);
            // now update the logs
            await this._userAuthLogRepo.SaveUserAuthLogAsync(userUpdateRequestDto.Email, UserEventType.Last_Update);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}