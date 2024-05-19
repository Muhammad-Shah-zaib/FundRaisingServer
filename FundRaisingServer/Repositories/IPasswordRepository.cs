namespace FundRaisingServer.Repositories;

public interface IPasswordRepository
{
    Task<bool> SaveUserPasswordAsync(int userCnic, string password);
    Task<bool> DeleteUserPasswordByEmailAsync(string email);
    Task<bool> DeleteUserPasswordByUserCnicAsync(int UserCnic);
}