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

}