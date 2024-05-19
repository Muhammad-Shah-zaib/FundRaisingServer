using System.Text;
using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace FundRaisingServer.Services;

// this Class will contain two methods for adding the user in db along with there passwords
public class UserService(FundRaisingDbContext context, IArgon2Hasher argon2Hasher) : IUserRepository
{
    // DIs
    private readonly FundRaisingDbContext _context = context;
    private readonly IArgon2Hasher _argon2Hasher = argon2Hasher;

    // this method will save the User in the database
    public async Task<bool> SaveUserAsync(RegistrationRequestDto user)
    {
        try
        {
            await this._context.Users.AddAsync(new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Cms = user.Cms.ToString(),
                PhoneNo = user.PhoneNo
            });
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
            await this._context.Passwords.AddAsync(new Password()
            {
                HashedPassword = Convert.ToBase64String(hashedPassword),
                HashKey = Convert.ToBase64String(salt),
                UserCnic = user.UserCnic
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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        // getting the user if exists
        const string query = $"Select * FROM Users Where Email = @UserEmail";
        var user = await this._context.Users.FromSqlRaw(query,
            new SqlParameter("@UserEmail", email))
            .FirstOrDefaultAsync();

        return user; // this return user or null
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        const string query = "SELECT * FROM Users WHERE User_CNIC = @userId";

        return await this._context.Users.FromSqlRaw(query,
            new SqlParameter("@userId", id))
            .SingleOrDefaultAsync();
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
        if (userFromDb == null) return false;

        // getting the passwords
        const string query = $"SELECT * FROM Passwords WHERE User_CNIC = @UserCnic";
        var userPassword = await this._context.Passwords.FromSqlRaw(query, new SqlParameter("@UserCnic", userFromDb.UserCnic)).FirstOrDefaultAsync();

        // checking the password
        var result = this._argon2Hasher.VerifyHash(
            Convert.FromBase64String(userPassword!.HashedPassword!),
                Convert.FromBase64String(userPassword!.HashKey!),
                Encoding.UTF8.GetBytes(inputPassword));
        return result;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        return await this._context.Users
            .Select(u => new UserResponseDto()
            {
                UserId = u.UserCnic,
                FirstName = u.FirstName!,
                LastName = u.LastName!,
                Email = u.Email,
                PhoneNo = u.PhoneNo,
                Cms = int.Parse(u.Cms),
                UserType = u.UserTypes.Where(ut => ut.UserCnic == u.UserCnic).Select(ut => ut.Type).SingleOrDefault() ?? null!,
                UserAuthLogsList = u.UserAuthLogs
                    .Where(l => l.UserCnic == u.UserCnic)
                    .Select(l => new UserAuthLogsResponseDto()
                    {
                        EventType = l.EventType,
                        EventTimestamp = l.EventTimestamp
                    })
                    .Distinct()
                    .ToList()
            })
            .ToListAsync();

    }

    /*
     * The method to update the user
     * tuple in the DB ...
     * you also need to update the log
     * of update... it is not implemented
     * in this method you can do this 
     * in controller
     */
    public async Task<bool> UpdateUserAsync(UserUpdateRequestDto userUpdateRequestDto)
    {
        const string query =
            "UPDATE Users SET First_Name = @First_Name, Last_Name = @Last_Name, Email = @Email WHERE User_CNIC = @UserCnic";

        // now we will update the user
        await this._context.Database.ExecuteSqlRawAsync(query,
            new SqlParameter("@First_Name", userUpdateRequestDto.FirstName),
            new SqlParameter("@Last_Name", userUpdateRequestDto.LastName),
            new SqlParameter("@Email", userUpdateRequestDto.Email),
            new SqlParameter("@UserId", userUpdateRequestDto.UserId));

        // now we need to add a log for updating the user
        await this._context.SaveChangesAsync();
        return true;


    }

    /*
     * The method below will delete the user
     * tuple in the DB ...
     */
    public async Task<bool> DeleteUserAsync(int userId)
    {
        try
        {
            const string query = "DELETE Users WHERE User_CNIC = @userId";

            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@userId", userId));
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}