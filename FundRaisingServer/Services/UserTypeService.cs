using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class UserTypeService(FundRaisingDbContext context, IUserRepository userRepo): IUserTypeRepository
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
            var user =  await this._userRepo.GetUserByEmailAsync(userTypeDto.Email);
            if (user == null) throw new Exception("No user find with the provided email");
            
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
}