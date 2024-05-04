using System.Text;
using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
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
            const string query = "INSERT INTO Users VALUES (@FirstName, @LastName, @Email)";
            await this._context.Database.ExecuteSqlRawAsync(query, 
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@Email", user.Email));
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
                        $"('{Convert.ToBase64String(hashedPassword)}', '{Convert.ToBase64String(salt)}', {user.UserCnic});";
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
        const string query = $"Select * FROM Users Where Email = @UserEmail";
        var user = await this._context.Users.FromSqlRaw(query,
            new SqlParameter("@UserEmail", email))
            .FirstOrDefaultAsync();
        
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
    
    
    /*
     * this method will check either
     * the user is present in the db
     * or not
     */
    public async Task<bool> CheckUserAsync(string email, string inputPassword)
    {
        // checking the user email
        var userFromDb = await this.GetUserByEmailAsync(email);
        if (userFromDb  == null) return false;
        
        // getting the passwords
        var query = $"SELECT * FROM Passwords WHERE User_ID = @UserId";
        var userPassword = await this._context.Passwords.FromSqlRaw(query, new SqlParameter("@UserId", userFromDb.UserCnic)).FirstOrDefaultAsync();
        
        // checking the password
        var result = this._argon2Hasher.VerifyHash(
            Convert.FromBase64String(userPassword!.HashedPassword!),
                Convert.FromBase64String(userPassword!.HashKey!),
                Encoding.UTF8.GetBytes(inputPassword));

        return result;
    }
}