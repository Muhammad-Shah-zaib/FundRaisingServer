using System.Text;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class PasswordService(FundRaisingDbContext context, IUserRepository userRepo, IArgon2Hasher argon2Hasher) : IPasswordRepository
{
    // DIs
    private readonly FundRaisingDbContext _context = context;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IArgon2Hasher _argon2Hasher = argon2Hasher;


    // saving the password of the user
    public async Task<bool> SaveUserPasswordAsync(int userCnic, string inputPassword)
    {
        try
        {
            // Hashing the password
            var password = Encoding.UTF8.GetBytes(inputPassword);
            var salt = Encoding.UTF8.GetBytes(RandomSaltGenerator.GenerateSalt(512 / 8));
            var hashedPassword = this._argon2Hasher.HashPassword(password: password, salt: salt);

            // saving User Password
            await this._context.Passwords.AddAsync(new Password()
            {
                HashedPassword = Convert.ToBase64String(hashedPassword),
                HashKey = Convert.ToBase64String(salt),
                UserCnic = userCnic
            });
            await this._context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteUserPasswordByEmailAsync(string email)
    {
        try
        {
            // get the user to get the id and then delete the user password from the table
            var user = await this._userRepo.GetUserByEmailAsync(email);
            if (user == null) return false;

            // deleting the user password from the table
            const string query = "DELETE Passwords WHERE User_CNIC = @UserCnic";

            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@UserCnic", user.UserCnic));
            await this._context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> DeleteUserPasswordByUserCnicAsync(int UserCnic)
    {
        try
        {
            const string deleteQuery = "DELETE Passwords WHERE User_CNIC = @UserCnic";
            await this._context.Database.ExecuteSqlRawAsync(deleteQuery,
                new SqlParameter("@UserCnic", UserCnic));
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}