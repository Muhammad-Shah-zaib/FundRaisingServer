namespace FundRaisingServer.Repositories;

public interface IPasswordRepository
{
    Task<bool> SaveUserPasswordAsync(string email, string password);
    Task<bool> DeleteUserPasswordByEmailAsync(string email);
}