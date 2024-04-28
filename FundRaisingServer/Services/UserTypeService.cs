using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class UserTypeService(FundRaisingDbContext context, IUserRepository userRepo) : IUserTypeRepository
{
    // DIs
    private readonly FundRaisingDbContext _context = context;
    private readonly IUserRepository _userRepo = userRepo;

    /*
     * The method below will first take the user
     * from th DB via userRepo and then use the
     * user_Id to store the correct userType corresponding
     * to correct user
     */
    public async Task<bool> SaveUserTypeAsync(UserTypeDto userTypeDto)
    {
        try
        {
            // getting the user 
            var user = await this._userRepo.GetUserByEmailAsync(userTypeDto.Email) ?? throw new Exception("No user find with the provided email");

            // saving the user
            const string query = $"INSERT INTO User_Type VALUES (@UserType, @UserId)";
            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@UserType", userTypeDto.UserType),
                new SqlParameter("@UserId", user.UserId));
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /*
    * The method below will delete 
    * the user type from the DB
    * by using the userId
    */
    public async Task<bool> DeleteUserTypeByUserIdAsync(int userId)
    {
        try
        {
            // deleting the user type from the table
            const string query = "DELETE User_Type WHERE User_ID = @UserId";
            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@UserId", userId));
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateUserTypeByUserIdAsync(int userId, string userType)
    {
        try
        {
            // adding the user type from the table
            const string query = "UPDATE User_Type SET Type = @UserType WHERE User_ID = @UserId";
            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@UserType", userType),
                new SqlParameter("@UserId", userId));
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