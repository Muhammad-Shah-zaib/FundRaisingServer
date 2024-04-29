using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class UserController(IUserRepository userRepo, IUserAuthLogRepository userAuthLogRepo, IPasswordRepository passwordRepo, IUserTypeRepository userTypeRepo) : ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IUserAuthLogRepository _userAuthLogRepo = userAuthLogRepo;
    private readonly IPasswordRepository _passwordRepo = passwordRepo;
    private readonly IUserTypeRepository _userTypeRepo = userTypeRepo;

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
    [Route("UpdateUser/{userId:int}")]

    /*
     * the api below is going to
     * update the user from the data
     * of type UpdateUserRequestDto
     * and then update the log of Update
     * and return the single updated User
     * with updated logs
     */
    public async Task<ActionResult<UserResponseDto>> UpdateUser([FromRoute] int userId, UserUpdateRequestDto userUpdateRequestDto)
    {
        /*
         * since there are already DataAnnotations
         * for the UpdateUserRequestDto so no need to
         * manually check the Model State
         */
        if (!ModelState.IsValid) return BadRequest("Please provide all the user details");

        try
        {
            // now we need to check either the given userId exist in Db
            // and the email corresponds to that userId or not?
            var user = await this._userRepo.GetUserByIdAsync(userId);
            if (user == null) return BadRequest($"There is no user with id {userId}");

            // now we need to check if the new email is already updated or not
            var userByEmail = await this._userRepo.GetUserByEmailAsync(userUpdateRequestDto.Email);
            if (userByEmail != null && userByEmail.UserId != userId) return StatusCode(409, "Email already exist.");

            userUpdateRequestDto.UserId = userId;
            /*
             * First we will update the tuple of users
             * so that we can have the updated Email,
             * and from the updated email we can add the logs
             */
            if (!await this._userRepo.UpdateUserAsync(userUpdateRequestDto)) return StatusCode(500, "Internal server error");
            // now update the logs
            if (!await this._userAuthLogRepo.SaveUserAuthLogAsync(userUpdateRequestDto.Email, UserEventType.Last_Update)) return StatusCode(500, "Internal server error");
            // now we can update the userType
            // since it is possible th euser dont want to update the userType
            // so we need to check it first
            if (userUpdateRequestDto.UserType != null)
                if (!await this._userTypeRepo.UpdateUserTypeByUserIdAsync(user.UserId, userUpdateRequestDto.UserType)) return StatusCode(500, "Internal server error");


            return Ok(new UserResponseDto()
            {
                FirstName = userUpdateRequestDto.FirstName,
                LastName = userUpdateRequestDto.LastName,
                Email = userUpdateRequestDto.Email,
                UserId = user.UserId,
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /*
     * the api below deletes the user
     * it also deletes all the tuples
     * present in UserType, UserAuthLog and Password
     * Tables that are associated with
     * the user provided to delete
     */
    [HttpDelete]
    [Route("DeleteUser/{userId:int}")]
    public async Task<ActionResult<UserResponseDto>> DeleteUser([FromRoute] int userId)
    {
        // first we will check if the user with userId exist or not
        var user = await this._userRepo.GetUserByIdAsync(id: userId);
        if (user == null) return BadRequest("User not Found");

        // since, now we know the user exist, so we can delete the user
        // DELETING THE LOGS
        if (!await this._userAuthLogRepo.DeleteUserAuthLogAsync(userId)) return StatusCode(500, "Internal server error");
        // DELETING THE PASSWORD
        if (!await this._passwordRepo.DeleteUserPasswordByUserIdAsync(userId)) return StatusCode(500, "Internal server error");
        // DELETING THE USER TYPE
        if (!await this._userTypeRepo.DeleteUserTypeByUserIdAsync(userId)) return StatusCode(500, "Internal server error");
        // DELETING THE USER
        if (!await this._userRepo.DeleteUserAsync(userId)) return StatusCode(500, "Internal server error");
        return Ok();

    }
}