using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace FundRaisingServer.Services;

public class UserAuthLogService(FundRaisingDbContext context, IUserRepository userRepo) : IUserAuthLogRepository
{
    private readonly FundRaisingDbContext _context = context;
    private readonly IUserRepository _userRepo = userRepo;
    public async Task<bool> SaveUserAuthLogAsync(string email, UserEventType eventTypeEnum)
    {
            Console.WriteLine(eventTypeEnum.GetDisplayName());
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
            
            // we will check if the user has already a log of UserEventType or not
            // this can only happened in case of already existing Last_Login event
            const string checkAuthLogQuery = "SELECT * FROM [dbo].[User_Auth_Log] WHERE [User_ID] = @UserId AND [Event_Type] = @EventType";
            var userAuthLog = await this._context.UserAuthLogs.FromSqlRaw(checkAuthLogQuery,
                new SqlParameter("@UserId", userAuthLogDto.UserId),
                new SqlParameter("@EventType", userAuthLogDto.EventType.GetDisplayName()))
                .SingleOrDefaultAsync();

            if (userAuthLog == null)
            {
                // while saving the event_type since it is an enum so we need to convert its value into string
                const string insertQuery = "INSERT INTO User_Auth_Log VALUES (@EventType, @EventTimestamp, @UserId) ";
                Console.WriteLine(userAuthLogDto.EventType.ToString());
                await this._context.Database.ExecuteSqlRawAsync(insertQuery,
                    new SqlParameter("@EventType", userAuthLogDto.EventType.GetDisplayName()),
                    new SqlParameter("@EventTimestamp", userAuthLogDto.EventTimestamp),
                    new SqlParameter("@UserId", userAuthLogDto.UserId));
                await this._context.SaveChangesAsync();
                return true;
            }

            const string updateQuery =
                "UPDATE User_Auth_log SET Event_Timestamp = DEFAULT  WHERE User_ID = @userId AND Event_Type = @eventType";

            await this._context.Database.ExecuteSqlRawAsync(updateQuery,
                new SqlParameter("@userId", user.UserId),
                new SqlParameter("@eventType", eventTypeEnum.GetDisplayName()));
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