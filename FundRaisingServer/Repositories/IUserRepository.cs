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
    
    // method below will get the user by id from db
    Task<User?> GetUserByIdAsync(int id);

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

    /*
     * The method below will delete the user
     * tuple in the DB ...
     * Before calling this function you need
     * to clear its passwords and userTypes
     * and logs, since they are associated with
     * the userId
     */
    Task<bool> DeleteUserAsync(int userId);
}