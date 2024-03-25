using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class UserAuthLogService (FundRaisingDbContext context, IUserRepository userRepo): IUserAuthLogRepository
{
    private readonly FundRaisingDbContext _context = context;
    private readonly IUserRepository _userRepo = userRepo;
    public async Task<bool> SaveUserAuthLogAsync(string email, UserEventType eventTypeEnum)
    {
        try
        {
            // getting the user to get its id
            var user = await this._userRepo.GetUserByEmailAsync(email);
            if (user == null) return false;
            
            /*
             * Now since we have the user we can get its id and
             * store its log
             */
            

            var userAuthLogDto = new UserAuthLogDto()
            {
                EventType = eventTypeEnum,
                EventTimestamp = DateTime.UtcNow,
                UserId = user.UserId
            };
            
            
            // while saving the event_type since it is an enum so we need to convert its value into string
            var query = $"INSERT INTO User_Auth_Log VALUES ('{userAuthLogDto.EventType.ToString()}', '{userAuthLogDto.EventTimestamp}', '{userAuthLogDto.UserId}') ";

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