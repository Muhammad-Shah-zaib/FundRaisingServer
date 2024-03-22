using System.Text;
using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

// this Class will contain two methods for adding the user in db along with there passwords
public class UserService(FundRaisingDbContext context, IArgon2Hasher argon2Hasher): IUserRepository
{
    // DIs
    private readonly FundRaisingDbContext _context = context;
    private readonly IArgon2Hasher _argon2Hasher = argon2Hasher;
    
    // this method will save the User in the database
    public async Task<bool> SaveUserAsync(RegistrationRequestDto user)
    {
        try
        {
            var query = $"INSERT INTO Users VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}')";
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
    
    // this method will save the password in the database
    public async Task<bool> SaveUserPasswordAsync(string email, string inputPassword)
    {
        try
        {
            // finding the user via Email
            var user = await this.GetUserByEmailAsync(email);

            // Extra check if the user exist or not
            if (user == null)
                return false;

            // Hashing the password
            var password = Encoding.UTF8.GetBytes(inputPassword);
            var salt = Encoding.UTF8.GetBytes(RandomSaltGenerator.GenerateSalt(512 / 8));
            var hashedPassword = this._argon2Hasher.HashPassword(password: password, salt: salt);

            // saving User Password
            var query = "INSERT INTO dbo.Passwords " +
                        "VALUES " +
                        $"('{Convert.ToBase64String(hashedPassword)}', '{Convert.ToBase64String(salt)}', {user.UserId});";
            await this._context.Database.ExecuteSqlRawAsync(query);
            await this._context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<User?> GetUserByEmailAsync( string email )
    {
        // getting the user if exists
        var query = $"Select * FROM Users Where Email = '{email}'";
        var user = await this._context.Users.FromSqlRaw(query).FirstOrDefaultAsync();
        
        return user; // this return user or null
    }

    public async Task<bool> DeleteUserByEmailAsync(string email)
    {
        try
        {
            var query = $"DELETE Users WHERE Email = '{email}'";
            await this._context.Database.ExecuteSqlRawAsync(query);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
    }
}