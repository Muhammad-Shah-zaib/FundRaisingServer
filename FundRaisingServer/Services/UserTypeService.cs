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
            await this._context.UserTypes.AddAsync(new UserType()
            {
                UserCnic = userTypeDto.UserCnic,
                Type = userTypeDto.UserType,

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

    /*
    * The method below will delete 
    * the user type from the DB
    * by using the UserCnic
    */
    public async Task<bool> DeleteUserTypeByUserCnicAsync(int UserCnic)
    {
        try
        {
            // deleting the user type from the table
            const string query = "DELETE User_Type WHERE User_CNIC = @UserCnic";
            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@UserCnic", UserCnic));
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateUserTypeByUserCnicAsync(int UserCnic, string userType)
    {
        try
        {
            // adding the user type from the table
            const string query = "UPDATE User_Type SET Type = @UserType WHERE User_CNIC = @UserCnic";
            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@UserType", userType),
                new SqlParameter("@UserCnic", UserCnic));
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