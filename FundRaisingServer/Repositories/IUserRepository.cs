using FundRaisingServer.Models.DTOs.UserAuth;

namespace FundRaisingServer.Repositories;

public interface IUserRepository
{ 
    // this method will save the user in the db
    Task<bool> SaveUserAsync(RegistrationRequestDto user);
    
    // this method will save the user Password in db
    Task<bool> SaveUserPasswordAsync(string email, string password);
    
    // this method will delete the user from the db
    Task<bool> DeleteUserByEmailAsync(string email);
    
    // this method will get the user by email from db
    Task<User?> GetUserByEmailAsync(string email);

    /*
     * This method will check Either
     * the user is present in Db or not
     * by using its email and password only
     */
    Task<bool> CheckUserAsync(string email, string password);

    /*
     * Method below will return all the users
     * including their logs
     */
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

    /*
     * This method update the user,
     * but you also have to update the logs
     * for upadting the logs you can
     * refer to IUserAuthLogRepository
     */
    Task<bool> UpdateUserAsync(UserUpdateRequestDto userUpdateRequestDto);
}