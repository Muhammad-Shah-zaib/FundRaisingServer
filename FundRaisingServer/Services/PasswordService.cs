using System.Text;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class PasswordService (FundRaisingDbContext context, IUserRepository userRepo, IArgon2Hasher argon2Hasher): IPasswordRepository
{
    // DIs
    private readonly FundRaisingDbContext _context = context;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IArgon2Hasher _argon2Hasher = argon2Hasher;


    // saving the password of the user
    /*
     * Since the Passwords has the User_ID as foreign key so we need
     * to get the user from the db hence we will get it by using Email
     * and we need userRepository DI for this. 
     */
    public async Task<bool> SaveUserPasswordAsync(string email, string inputPassword)
    {
        try
        {
            // finding the user via Email
            var user = await this._userRepo.GetUserByEmailAsync(email);

            // Extra check if the user exist or not
            if (user == null)
                return false;
            // Hashing the password
            var password = Encoding.UTF8.GetBytes(inputPassword);
            var salt = Encoding.UTF8.GetBytes(RandomSaltGenerator.GenerateSalt(512 / 8));
            var hashedPassword = this._argon2Hasher.HashPassword(password: password, salt: salt);

            // saving User Password
            var query = @$"
                        INSERT INTO dbo.Passwords
                        VALUES 
                             ('{Convert.ToBase64String(hashedPassword)}', '{Convert.ToBase64String(salt)}', {user.UserId});
                        ";
            await this._context.Database.ExecuteSqlRawAsync(query);
            await this._context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}